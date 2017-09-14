using Loria.Google;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LoriaNET.Location
{
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
        
        public override string Name => "Presence module";

        public string Description => "Tell Loria where you are!";

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

        public LocationModule(Configuration configuration) 
            : base(configuration)
        {
            Positions = new Positions();
            Places = new Places();
            
            GoogleMap = new GoogleMap(Configuration.Get("google::ApiKey"));
        }
        
        public override void Configure()
        {
            // TODO: will further need to configure the location module
            // with a discussion between the user and Loria

            // Add manager's home
            Positions.Add(Configuration.Manager);
            Positions.AddRange(Configuration.Contacts);

            // Handle Loria contacts change
            Configuration.Contacts.CollectionChanged += Contacts_CollectionChanged;

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
                    var newPerson = e.NewItems.OfType<Person>().FirstOrDefault();
                    if (newPerson != null)
                    {
                        Positions.Add(newPerson);
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    var oldPerson = e.OldItems.OfType<Person>().FirstOrDefault();
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
                Configuration.Hub.PropagateCallback("You didn't tell me a person");
                return;
            }

            var coordinatesEntity = command.GetEntity(CoordinatesEntity);
            if (coordinatesEntity == null)
            {
                Configuration.Hub.PropagateCallback("You didn't tell me coordinates");
                return;
            }

            var person = Configuration.GetPerson(forEntity.Value);
            if (person == null)
            {
                Configuration.Hub.PropagateCallback("I've never met this person.");
                return;
            }

            Positions.SetCoordinate(person, new Coordinates(coordinatesEntity.Value));
        }

        public void Get(Command command)
        {
            var forEntity = command.GetEntity(ForEntity);
            if (forEntity == null)
            {
                Configuration.Hub.PropagateCallback("You didn't tell me a person");
                return;
            }

            var person = Configuration.GetPerson(forEntity.Value);
            if (person == null)
            {
                Configuration.Hub.PropagateCallback("I've never met this person.");
                return;
            }

            var position = Positions.Get(person);
            Configuration.Hub.PropagateCallback(position.ToString());
        }

        public void Leave(Command command)
        {
            var forEntity = command.GetEntity(ForEntity);
            if (forEntity == null)
            {
                Configuration.Hub.PropagateCallback("You didn't tell me a person");
                return;
            }

            var placeEntity = command.GetEntity(PlaceEntity);
            if (placeEntity == null)
            {
                Configuration.Hub.PropagateCallback("You didn't tell me a place");
                return;
            }

            var person = Configuration.GetPerson(forEntity.Value);
            if (person == null)
            {
                Configuration.Hub.PropagateCallback("I've never met this person.");
                return;
            }

            var place = Places.Get(placeEntity.Value);
            if (place == null)
            {
                Configuration.Hub.PropagateCallback("I don't know this place");
                return;
            }

            Positions.LeavePlace(person, place);
        }

        public void Enter(Command command)
        {
            var forEntity = command.GetEntity(ForEntity);
            if (forEntity == null)
            {
                Configuration.Hub.PropagateCallback("You didn't tell me a person");
                return;
            }

            var placeEntity = command.GetEntity(PlaceEntity);
            if (placeEntity == null)
            {
                Configuration.Hub.PropagateCallback("You didn't tell me a place");
                return;
            }

            var person = Configuration.GetPerson(forEntity.Value);
            if (person == null)
            {
                Configuration.Hub.PropagateCallback("I've never met this person.");
                return;
            }

            var place = Places.Get(placeEntity.Value);
            if (place == null)
            {
                Configuration.Hub.PropagateCallback("I don't know this place");
                return;
            }

            Positions.EnterPlace(person, place);
        }
        
        public void Add(Command command)
        {
            var coordinatesEntity = command.GetEntity(CoordinatesEntity);
            if (coordinatesEntity == null)
            {
                Configuration.Hub.PropagateCallback("You didn't tell me coordinates");
                return;
            }

            var placeEntity = command.GetEntity(PlaceEntity);
            if (placeEntity == null)
            {
                Configuration.Hub.PropagateCallback("You didn't tell me a place");
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
                Configuration.Hub.PropagateCallback("You didn't tell me a place");
                return;
            }

            Places.Remove(placeEntity.Value);
        }
    }
}
