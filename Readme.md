# Intro

This solution provides a simple implementation of a Gomoku game as a web service.
The move order is controlled by the service, and the winner is also determined after the
winning stone is placed.

# Service Endpoints
Web service consists of three calls:
1. Status: provides the game status and overview of the board.
2. PlaceStone: places a stone on the board. It takes and object with row and column indexes as a parameter.
   Indexes start at 0. Colour of the stone placed is tracked by the service itself.
3. Restart: returns the game to its initial state with an empty board and first move granted to "Black". 

Swagger is left enabled in this service for ease of checking and interacting with an API.

# Structure
_GomokuGameAPI_ service project only implements thin layer for request processing and for everything else
relies on the _GomokuGame_ project.
