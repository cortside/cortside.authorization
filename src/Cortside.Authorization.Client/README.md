# Authorization.Client

To be installed by consuming services.

```
services.AddAuthorizationApiClient(Configuration);
```

Example configuration section needed:

```
"AuthorizationApi": {
    "ServiceUrl": "http://localhost:5044",
    "PolicyResourceId": "15F50AAB-5FBD-406E-A1E0-BDC961B4CDBA", // should match the desired PolicyResourceId established in the DB for the calling service's policy
    "PolicyName": "CatalogApi",
    "ClaimTypes": [ "sub", "groups" ], // should match claimtypes configured in the DB for the calling service's policy for more efficient evaluation
    "CacheDuration": "0.00:05:00"
},
"AccessControl": {
    "AuthorizationProvider": "AuthorizationApi"
},
```
