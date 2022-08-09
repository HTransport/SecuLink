# SecuLink 1.1
Top-notch service when it comes to your own vault.

Web API built with ASP.NET Entity Framework Core 5, has all the basic functions for your users and cards.

NOTE: ACCEPTS REQUESTS FROM http://localhost:3000 AND ITSELF ONLY !!!

# Models

Each has its own table in the Database

User
- Id (autonumber, not required)
- Username
- Password

Card
- Id (autonumber, not required)
- SerialNumber
- UserId (FK)

Token
- Id (autonumber, not required)
- Content
- TTL_seconds
- DOC (Date of creation)
- UserId (FK)

NewUser
- Id (autonumber, not required)
- Username
- Pin (New user enters it to finish signup)

Admin
- Id (autonumber, not required)
- UserId (FK)

# RequestModels

Used to send specific requests to certain endpoints

UTE
- Username
- Password
- Token (Content)
- EId (Id of entity making a change)

NTE
- Username
- Token (Content)
- EId (Id of entity making a change)

NUTE
- Username
- Pin
- Password
- Token (Content)
- EId (Id of entity making a change)

UEX
- Username
- Password
- Key (Private admin key - set by the admins themselves)

CTE
- SerialNumber
- UserId
- Token (Content)
- EId (Id of entity making a change)

# Controllers

Each have their own endpoints that send responses to requests while performing specific tasks

- UserController
- CardController
- TokenController
- AuthController

# UserController

api/user

Used to manage Users and NewUsers


CreateUser - /create
- POST, NUTE
- returns User object that is created and stored

- to use this Action, it's crucial that you create a NewUser beforehand

CreateUserDirect - /create/root
- POST, UEX
- root Action, generally used to create new Admin Users (you need to Authorize them separately though)
- returns User object that is created and stored


CreateNew - /create/new
- POST, NTE
- returns NewUser object
- used to create accounts for clients, client set the password by themselves using the Pin provided


DeleteUser - /delete
- POST, UTE
- returns "Deleted User and its content" if the deletion was successful
- deletes the User and any Card, Token and/or Auth (Unauthorizes the User)


DeleteNewUser - /delete/new
- POST, NTE
- returns 'true' if the deletion was successful


GetUserByUsername - /get
- POST, NTE
- returns a complete User object


GetNewUserByUsername - /get/new
- POST, NTE
- returns a complete NewUser object


Login - /login
- POST, User
- creates/overrides a Token and returns its Content


# CardController

api/card

Used to manage Cards


CreateCard - /create
- POST, CTE
- returns Card object created, but if a Card already exists bound to User then returns 'false'
	- only one Card per User


DeleteCard - /delete
- POST, NTE
- returns 'true' if the deletion was successful


GetCardByUsername - /get
- POST, NTE
- returns a complete Card object, and strings "User with bound card doesn't exist" or "Card doesn't exist" in case of such errors


# TokenController

api/token

Used to manage Tokens, designed to only be accessible to admins since Users already have Login to create their own Tokens


CreateToken - /{UserId}
- GET
- returns Content of created Token
- can be used to prolong existing Login sessions


CheckExpiration - /exp/{UserId}
- GET
- returns 'true' if the Token is valid, otherwise 'false' (and, if needed, deletes said Token)
- can be used to clear out unnecessary Tokens for Users that haven't logged in for a while, whilst failing to activate ATEC-s (Automatic Token Expiration Checks)


DeleteToken - /{UserId}
- DELETE
- returns 'true' if deleted, otherwise 'false' (doesn't exist or already deleted)
- can be used to log out Users using a 'Log Out' button

# AuthController

api/auth

Used to authorize Users with Admin privileges, of course designed as inaccessible to basic Users


AuthorizeUser - /op
- POST, NTE
- returns 'true' if authorization succeds, otherwise 'false'


UnauthorizeUser - /deop
- POST, NTE
- returns 'true' if unauthorization succeds, otherwise 'false'


CheckUserAuth - /check
- POST, NTE
- returns 'true' if user is authorized, otherwise 'false'

# Other Info

Default Token expiration response - "e pa nemre"

All requests are HTTP

Unfortunately, a huge portion of the endpoints accept only POST since they need Tokens for security reasons

SQL DB - SecuLink.bak