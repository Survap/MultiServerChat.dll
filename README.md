# MultiServerChat
Originally made by Zack Piispanen, now maintained and updated by Ryozuki

## Commands:
Command | Permission | Arguments | Description
--- | --- | --- | ---
/msc_reload | msc.reload | | Reloads the config, specially needed to add the rest token.

You need to add the rest token in the configuration file.

Setting up rest: https://tshock.readme.io/v4.3.24/reference

If you want to have a token that persists between server restarts add it at tshock config.json
Find "ApplicationRestTokens":
```JSON
"ApplicationRestTokens": {
    "TOKENYOULIKE": {
      "Username": "ValidUsername",
      "UserGroupName": "ValidGroup"
    }
}
For example:
"ApplicationRestTokens": {
    "TOKENYOULIKE": {
      "Username": "Ryozuki",
      "UserGroupName": "superadmin"
    }
}
```
