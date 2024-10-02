# StadiumRentalClient

Despite the name, this app does not use a client/server setup. I just named it that to keep the purpose of this app and its companion clear in my mind.

This app was made specifically to help facilitate a Pokemon Stadium tournament hosted by https://www.youtube.com/@DillonWithaBlankie. If anyone else wishes to use it, you will need to set up a MongoDB cluster because this app and its companion app utilize a MongoDB cluster as an intermediary to communicate. The connection string to the Mongo cluster needs to be stored in a text file named "ConnectionString" and be placed in the same directory as the app. The name of the connection string file must not include a file extension.

This app is meant to be used by the tournament players. The app is divided into two tabs. The first tab is used by players to set the party that they will use in the tournament. The list of available pokemon for players to choose from must be included within the database. Once a player has saved their party to the database, they may swap over to the second tab for tournament play. In the second tab, players can choose the lineup of pokemon to take into a match, as well as select their input and send it to the database.
