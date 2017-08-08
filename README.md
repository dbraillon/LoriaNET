Les actions sont appelés par des commandes
  - Parmi les actions, il existe les callbacks, ce sont des actions spécialisé dans la transmition des données à l'utilisateur
Les listeners sont des thread qui attende que quelque chose se passe
  - ils transmette des commandes
  - ils transmettent des callbacks
Les modules sont des classes qui définissent une action, un callback ou et un listener

exemples :

  - la console est un listener qui écoute des commandes
    les commandes sont transmis au hub des actions pour déclencher l'action corresp

  - la console est aussi un callbacks

  - un server http peut etre un listeners
