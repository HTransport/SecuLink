# SecuLink
Top-notch service when it comes to your own vault.

Web API built with ASP.NET Entity Framework Core 5, has all the basic functions for your users and cards.

# Models
User
- Id (autonumber, not required)
- Username
- Password_Enc (encryption coming soon)

Card
- Id (autonumber, not required)
- SerialNumber
- Pin (functionality coming soon, currently 0 for all)
- UserId (FK)

# Endpoints - User Manipulation
CreateUser(User) - POST
localhost:port/api/user 
- accepts JSON
- returns complete user object

DeleteUser(Username) - DELETE
localhost:port/api/user/{Username}
- returns string
 - "Deleted User" if no card is found that binds to that user
 - "Deleted User and Card" if a card was found
- deletes a card related to that user if one exists

GetUserByUsername(Username) - GET
localhost:port/api/user/g/{Username}
- returns complete user object

Login(User) - POST
localhost:port/api/user/login
- accepts JSON
- returns bool

# Endpoints - Card Manipulation
CreateCard(Card) - POST
localhost:port/api/card
- accepts JSON
- returns complete card object

DeleteCard(SerialNumber [deletion parameter will be changed to 'Username' soon]) - DELETE
localhost:port/api/card/{SerialNumber}
- returns bool

GetCardByUsername(Username) - GET
localhost:port/api/card/{Username}
- returns complete card and related user objects