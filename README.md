# Goonsquad-me

A bunch of goons is a goonsquad.

# Getting Started

Install dependencies

```
dotnet restore
```

Apply migrations

```
dotnet ef database update
```

# Deployment

Resources you'll need to setup beforehand:

* RawgAPI
* Auth0
* SignalR
* PostgreSQL

Configure the following in your `appsettings.json` file or as environment variables (depending on how you're deploying things)

```
{
    "RawgApiKey": "",
    "Auth0": {
        "Domain": "",
        "Audience": "",
        "ManagementApiAudience": "",
        "ClientId": "",
        "ClientSecret": ""
    },
    "ConnectionStrings": {
        "SignalR": "",
        "PostgreSQL": ""
    }
}
```

The above as environment variables

```
RawgApiKey=""

Auth0__Domain=""
Auth0__Audience=""
Auth0__ManagementApiAudience=""
Auth0__ClientId=""
Auth0__ClientSecret=""

ConnectionStrings__SignalR=""
ConnectionStrings__PostgreSQL=""
```

And to put everything together

```
dotnet publish
```
