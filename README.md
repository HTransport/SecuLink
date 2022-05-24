# SecuLink 1.02
Top-notch service when it comes to your own vault.

Web API built with ASP.NET Entity Framework Core 5, has all the basic functions for your users and cards.

# Models
User
- Id (autonumber, not required)
- Username
- Password_Enc

Card
- Id (autonumber, not required)
- SerialNumber
- Pin (functionality coming soon, currently 0 for all)
- UserId (FK)

# Endpoints - User Manipulation
CreateUser(User) - POST
localhost:port/api/user 
- accepts JSON
- returns complete user object, in case of error returns error name in Username of object

 -> "User already exists" if user exists

DeleteUser(Username) - DELETE
localhost:port/api/user/{Username}
- returns string

 -> "User doesn't exist or already deleted" if user not found

 -> "Deleted User" if no card is found that binds to that user

 -> "Deleted User and Card" if a card was found
- deletes a card related to that user if one exists

GetUserByUsername(Username) - GET
localhost:port/api/user/{Username}
- returns complete user object, in case of error returns error name in Username of object

 -> "User doesn't exist" if user is null

Login(User) - POST
localhost:port/api/user/login
- accepts JSON
- returns bool

# Endpoints - Card Manipulation
CreateCard(Card) - POST
localhost:port/api/card
- accepts JSON
- returns complete card object, in case of error returns error name in SerialNumber of object

 -> "Card already exists" if card exists

DeleteCard(SerialNumber [deletion parameter will be changed to 'Username' soon]) - DELETE
localhost:port/api/card/{SerialNumber}
- returns bool

GetCardByUsername(Username) - GET
localhost:port/api/card/{Username}
- returns complete card and related user objects, in case of error returns error name in SerialNumber of object

 -> "User with bound card doesn't exist" if user to which card is bound is null

 -> "Card doesn't exist" if card is null