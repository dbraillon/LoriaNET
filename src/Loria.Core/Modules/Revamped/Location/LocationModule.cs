using Loria.Google;
using LoriaNET.Core.Database;
using LoriaNET.Resources;
using LoriaNET.Storage;
using LoriaNET.Storage.Database;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LoriaNET.Location
{
    /// <summary>
    /// The location module provides intents and entities to keep track of people location.
    /// </summary>
    public class LocationModule : Module, IAction
    {
        public const string SetIntent = "set";
        public const string GetIntent = "get";
        public const string LeaveIntent = "leave";
        public const string EnterIntent = "enter";
        public const string AddIntent = "add";
        public const string RemoveIntent = "remove";

        public const string ForEntity = "for";
        public const string PlaceEntity = "place";
        public const string CoordinatesEntity = "coordinates";
        
        public override string Name => Strings.LocationModuleName;

        public string Description => Strings.LocationModuleDescription;

        public string Command => "location";

        public string[] SupportedIntents => new string[]
        {
            SetIntent, GetIntent, LeaveIntent, EnterIntent, AddIntent, RemoveIntent
        };

        public string[] SupportedEntities => new string[]
        {
            ForEntity, PlaceEntity, CoordinatesEntity
        };

        public string[] Samples => new string[] 
        {
            "location set -for Davy -coordinates 42.2069426,5.7328107",
            "location get -for Davy",
            "location left -for Davy -place home",
            "location enter -for Davy -place work",
            "location add -place work -coordinates 42.2069426,5.7328107",
            "location remove -place work"
        };

        public Positions Positions { get; set; }
        public Places Places { get; set; }

        public GoogleMap GoogleMap { get; set; }
        public Timer GoogleMapTimer { get; set; }

        public LocationModule(Loria loria) 
            : base(loria)
        {
            Positions = new Positions();
            Places = new Places();
            
            GoogleMap = new GoogleMap(Loria.Data.ConfigurationFile.Get("google::ApiKey"));
        }
        
        public override void Configure()
        {
            // TODO: will further need to configure the location module
            // with a discussion between the user and Loria

            // Add manager's home
            Positions.Add(Loria.Data.Manager);
            Positions.AddRange(Loria.Data.Contacts);

            // Handle Loria contacts change
            Loria.Data.Contacts.CollectionChanged += Contacts_CollectionChanged;

            // Start if available google map reverse geocoding thread
            if (GoogleMap.IsConfigured)
            {
                GoogleMapTimer = new Timer(async (state) =>
                {
                    foreach (var place in Places.Items)
                    {
                        await (place.Coordinates?.ReverseGeocodingAsync(GoogleMap) ?? Task.FromResult(0));
                    }

                    foreach (var position in Positions.Items)
                    {
                        await (position.Coordinates?.ReverseGeocodingAsync(GoogleMap) ?? Task.FromResult(0));
                        await (position.Place?.Coordinates?.ReverseGeocodingAsync(GoogleMap) ?? Task.FromResult(0));
                    }
                }, "update", TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(30)); 
            }

            // Activate the module
            Activate();
        }

        /// <summary>
        /// Handle when Loria meet a new person or forget someone.
        /// </summary>
        private void Contacts_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    var newPerson = e.NewItems.OfType<PersonEntity>().FirstOrDefault();
                    if (newPerson != null)
                    {
                        Positions.Add(newPerson);
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    var oldPerson = e.OldItems.OfType<PersonEntity>().FirstOrDefault();
                    if (oldPerson != null)
                    {
                        Positions.Remove(oldPerson);
                    }
                    break;
            }
        }

        public void Perform(Command command)
        {
            switch (command.Intent)
            {
                case SetIntent:
                    Set(command);
                    break;

                case GetIntent:
                    Get(command);
                    break;

                case LeaveIntent:
                    Leave(command);
                    break;

                case EnterIntent:
                    Enter(command);
                    break;

                case AddIntent:
                    Add(command);
                    break;

                case RemoveIntent:
                    Remove(command);
                    break;
            }
        }

        public void Set(Command command)
        {
            var forEntity = command.GetEntity(ForEntity);
            if (forEntity == null)
            {
                Loria.Hub.PropagateCallback(Strings.LocationModulePersonNotFound);
                return;
            }

            var coordinatesEntity = command.GetEntity(CoordinatesEntity);
            if (coordinatesEntity == null)
            {
                Loria.Hub.PropagateCallback(Strings.LocationModuleCoordinatesNotFound);
                return;
            }

            var person = Loria.Data.GetPerson(forEntity.Value);
            if (person == null)
            {
                Loria.Hub.PropagateCallback(Strings.LocationModulePersonUnknown);
                return;
            }

            Positions.SetCoordinate(person, new Coordinates(coordinatesEntity.Value));
        }

        public void Get(Command command)
        {
            var forEntity = command.GetEntity(ForEntity);
            if (forEntity == null)
            {
                Loria.Hub.PropagateCallback(Strings.LocationModulePersonNotFound);
                return;
            }

            var person = Loria.Data.GetPerson(forEntity.Value);
            if (person == null)
            {
                Loria.Hub.PropagateCallback(Strings.LocationModulePersonUnknown);
                return;
            }

            var position = Positions.Get(person);
            Loria.Hub.PropagateCallback(position.ToString());
        }

        public void Leave(Command command)
        {
            var forEntity = command.GetEntity(ForEntity);
            if (forEntity == null)
            {
                Loria.Hub.PropagateCallback(Strings.LocationModulePersonNotFound);
                return;
            }

            var placeEntity = command.GetEntity(PlaceEntity);
            if (placeEntity == null)
            {
                Loria.Hub.PropagateCallback(Strings.LocationModulePlaceNotFound);
                return;
            }

            var person = Loria.Data.GetPerson(forEntity.Value);
            if (person == null)
            {
                Loria.Hub.PropagateCallback(Strings.LocationModulePersonUnknown);
                return;
            }

            var place = Places.Get(placeEntity.Value);
            if (place == null)
            {
                Loria.Hub.PropagateCallback(Strings.LocationModulePlaceUnknown);
                return;
            }

            Positions.LeavePlace(person, place);
        }

        public void Enter(Command command)
        {
            var forEntity = command.GetEntity(ForEntity);
            if (forEntity == null)
            {
                Loria.Hub.PropagateCallback(Strings.LocationModulePersonNotFound);
                return;
            }

            var placeEntity = command.GetEntity(PlaceEntity);
            if (placeEntity == null)
            {
                Loria.Hub.PropagateCallback(Strings.LocationModulePlaceNotFound);
                return;
            }

            var person = Loria.Data.GetPerson(forEntity.Value);
            if (person == null)
            {
                Loria.Hub.PropagateCallback(Strings.LocationModulePersonUnknown);
                return;
            }

            var place = Places.Get(placeEntity.Value);
            if (place == null)
            {
                Loria.Hub.PropagateCallback(Strings.LocationModulePlaceUnknown);
                return;
            }

            Positions.EnterPlace(person, place);
        }
        
        public void Add(Command command)
        {
            var coordinatesEntity = command.GetEntity(CoordinatesEntity);
            if (coordinatesEntity == null)
            {
                Loria.Hub.PropagateCallback(Strings.LocationModuleCoordinatesNotFound);
                return;
            }

            var placeEntity = command.GetEntity(PlaceEntity);
            if (placeEntity == null)
            {
                Loria.Hub.PropagateCallback(Strings.LocationModulePlaceNotFound);
                return;
            }

            var place = new Place(new Coordinates(coordinatesEntity.Value), placeEntity.Value);
            Places.Add(place);
        }

        public void Remove(Command command)
        {
            var placeEntity = command.GetEntity(PlaceEntity);
            if (placeEntity == null)
            {
                Loria.Hub.PropagateCallback(Strings.LocationModulePlaceNotFound);
                return;
            }

            Places.Remove(placeEntity.Value);
        }
    }
}
