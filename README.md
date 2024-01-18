# Flabs.Configuration.Vault

### What is Flabs.Configuration.Vault?
Enables HashiCorp Vault to be used as a config in .NET applications
Inspired by [VaultSharp](https://github.com/rajanadar/VaultSharp) 
Flabs.Configuration.Vault is an extension that using vault as configuration. 
.NET Standard 2.0 and 2.1 based cross platform C# library.

### Getting started

`dotnet add package Flabs.Configuration.Vault.Extensions`

Configure your own MountPoint in vault KV or default is : flabs.kv

Configure DI using VaultOptions
```csharp
var flabsConfig = new FlabsConfigOptions("root", "http://localhost:8200/");

builder.Services.AddFlabsConfig(vaultOptions);
```

Configure DI by Environment variables
```csharp
builder.Services.AddFlabsConfig();
builder.Services.AddConfigOptions<SampleOptions>();
```
Setup your environment variables 
VAULT_ADDR='you_vaultendpoint'
VAULT_TOKEN='token'

Adding Config file
```csharp
builder.Services.AddConfigOptions<SampleOptions>();
```

Get Config file
```csharp
SampleOptions sampleOpt = _serviceProvider.GetConfig<SampleOptions>();
```


### Features
* Setup default vault path to {Assembly.Name}/ConfigName
* Create vault path automatically if not found
* Add reload config by background job
