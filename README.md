#Introduction

Project is going to be used for educational purposes for a PhD thesis that focus on studying impact of multiplayer games in developing social skills. To make a safe environment for early school students, custom made game have to be developed.  Pre and posttest questionnaires will be used as a main research tools. Additional documentation may include video recording or logs. About 20 students will take part in research at time.


##Scenario of lesson [User case scenario]
1.	Downloading and unpacking app
2.	Running exe and connecting to server
3.	Creating or joining lobby
4.	Setting up game mode (?) [Host]
5.	Setting up names/characters/teams (?) [Client] – depending on game mode
6.	Starting match 
7.	Popup with rules to accept and set game player ready
8.	After all players are ready match starts
--- GAMEPLAY --
9.	After reaching required score – ending match, displaying scoreboard, returning to lobby
10.	Repeat with different map for about 3-4 attempts depending on how time consuming it will be.

##Game Modes (levelController)
1.	Farm Apples 

Team based game (2-10 players per match). Basic score collecting game – players have to pick up spawned item and bring it to team box to collect it and increase a score of a team.

Additional pickups: bonus speed, stealing mask (other ideas postponed till project reach decent state)

Difficulties: 
•	team assign - no spectator mode

o	v1 – equal teams depending of amount of players
2 – 2 teams, 3 – 3 teams – 4 – 2 teams, 5 – 5 teams, 6 – 2 teams , 7 – ughh 7 teams, 8 – 2 teams, 9 – 3 teams, 10 – 2 teams. (can be limited to 4, 6, 8, 9, 10 players to start)

o	v2 asymmetrical 
up to 1 more player with “equal opportunities” speed bonus) 
To do: 
o	match cleanup
o	additional UI canvas for rules and scoreboard
o	additional maps
o	sound
o	postprocessing
o	polishing

2.	Farmaggle 

Card trading game, every player have 5 random cards (food types) and can trade with any other player to get a set that is required for a gameplay. (everyone can trade with everyone it’s not a turn based, strategy heavy game, more casual, focused on walking and asking others what they have or need)

Difficulties:

•	Card generation without repetition ( possibly reducing card types to make it easier)
•	Avoiding random sets that may be impossible to get for more players
To do:
o	Maps
o	Trade UI
o	Emote UI

3.	Farm helper

Quiz based game, every player goes on their own after village to help NPCs, after picking right response player gets a reward. More as shared world experience, text chat may be useful for communication and looking for help from others. 

Difficulties:
To do:
o	Map
o	Dialogue UI
o	Inventory/Ojective UI
o	Text chat

4.	Farmaze

2 player asymmetrical gameplay mode, one person goes through labyrinth, another one has a role of a navigator ( 1st person uses 3rd person controller, 2nd use top view camera and UI to send direction icon for 1st player ) 

Difficulties:
•	random maze generation (may not be really necessary if at least 10 handmade mazes)
To do:
o	UI for Navigator
o	Message system to send from P2 to P1
o	Maps

5.	Farmarathon

Another team focused activity with different roles. (6,8,9 players)
1 player carrying item that has to reach destination, others can help or disturb other carries.
