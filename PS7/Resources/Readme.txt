Authors: Trung Le and Adam Sorensen
11/11/2015
CS 3500
PS7 - AgCubio

11/11/2015
	Program that creates a client that connects to a server to simulate a game similar to Agario.
	The AgCubio model contains the cube and world classes. So far have not dealt with the world class. 
	We got the client to succesfully connect to the server and to send a player name to the server. When opening the example client we were able to see 
	a cube representing the player name that we created in our client. 
	
	Needed a way to get the string containing data that is sent from the server from the network code to the gui code.
	Did this by using the same callback method for that is used for connect_to_server. 
	First time the callback is called in connected_to_server that callback method sends the player name to the server and prints a message box saying it is connected
	Second time the callback is called then we have the state with the json style string for the player cube
	Tried to deserialize it into a cube object but failed to get it to parse properly. It only parses the name and the mass.
	The DrawCube method also does not work with the new 8 parameter cube class. 

11/12/2015
	Fixed the issues with the json serialize/deserialize not parsing correctly. Added {get, set} for all member variables of the cube class. 
	We were able to send in the player name and then receive back JSON string data from the server. The data string usually sends enough data for at least 2 cube objects.
	Able to cut the data string so that it isolates the first cube with the player name. 
	Able to deserialize the data string into a cube object and send it into the local drawCube method which draws the cube.