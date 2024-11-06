# UsersProvider_Rika

Functions  keys:
DeleteUser:https://usersprovider.azurewebsites.net/api/users/{id}?  ,  
GetOneUser:https://usersprovider.azurewebsites.net/api/users/{id}?  ,  
GetUsers:https://usersprovider.azurewebsites.net/api/GetUsers?code=HKEdVTjpyNfe5mTbmi35Nhbc1EUpfaFEtv9ppxjmjDi0AzFuBTcLvA%3D%3D   ,  
UpdateImage:https://usersprovider.azurewebsites.net/api/usersprofile/{id}?code=GD_Vh8_dIdIUTTsjqroQivZHQB8CwhVHue44f8oYPyGmAzFuNqvwmw%3D%3D  ,  
UpdateUser:https://usersprovider.azurewebsites.net/api/users/{userId}?code=nXNo94PHN_SybIa19GT21JE5Xo5fe4QDcOO3thqkzYSQAzFuDfHVBA%3D%3D  ,  
UpdateUsersRoles:https://usersprovider.azurewebsites.net/api/usersrole/{userId}?code=I-Oj52eBPeOttj1VukKlJu5SGk2jB5d6TxsGcfanizzHAzFuov4PbQ%3D%3D  , 


UserEntity:
{    
    FirstName
    LastName 
    Address 
    City
    PostalCode
    Country
    (int) Age
    (int) GenderId 
    (int) LanguageId
    ImageUrl 
}

Update User ---->
{
"аddress": ""  ,
"postalcode": ""  ,
"city" : ""  ,
"country" : "Sweden"  ,
"firstname" : ""  ,
"lastname" :""  ,
"imageUrl" : "" 
}

RolesId: 
50515b79-82d8-46bc-9731-bdcaf94f77af  -- Admin  ,
5e7fe8e4-3350-4130-8aa7-1ec8c7712a67  --  Manager  , 
8b24f673-b180-4876-a7a7-7e780cc85526 -- SuperAdmin  ,
fba86002-a90d-496a-b0cb-afe2cc6f96ca  -- User   ,
