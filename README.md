# About
Server side of future bot, with this bot you can play tic-tac-toe either with AI or other player.

# Architecture
```
Player
-SendMessage

AI
-Process

Player : AI
Player : User

Room
-Player
-Player
-UpdateState

Telegram
-Send
-Receiver

Game
-Room[]
-StartGame
-Telegram
```