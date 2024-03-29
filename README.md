# Glitched Epistle

[![NuGet](https://buildstats.info/nuget/GlitchedPolygons.GlitchedEpistle.Client)](https://www.nuget.org/packages/GlitchedPolygons.GlitchedEpistle.Client)
[![API Docs](https://img.shields.io/badge/api-docs-informational)](https://glitchedpolygons.github.io/GlitchedEpistle.Client/api/index.html)
[![AppVeyor](https://ci.appveyor.com/api/projects/status/1nbq83g1iqibs6hr/branch/master?svg=true)](https://ci.appveyor.com/project/GlitchedPolygons/glitchedepistle-client/branch/master)
[![Travis Build Status](https://travis-ci.org/GlitchedPolygons/GlitchedEpistle.Client.svg?branch=master)](https://travis-ci.org/GlitchedPolygons/GlitchedEpistle.Client)
[![License Shield](https://img.shields.io/badge/license-GPLv3-brightgreen)](https://github.com/GlitchedPolygons/GlitchedEpistle.Client/blob/master/LICENSE)

## Shared codebase for implementing clients

Technology used:
* C# 8.0 ([netstandard2.1](https://github.com/dotnet/standard/blob/master/docs/versions/netstandard2.1.md))
* [RestSharp](https://github.com/restsharp/RestSharp)
* [GlitchedPolygons.RepositoryPattern.SQLite](https://github.com/GlitchedPolygons/RepositoryPattern.SQLite)
* [GlitchedPolygons.Services.CompressionUtility](https://github.com/GlitchedPolygons/CompressionUtility)
* [GlitchedPolygons.Services.Cryptography.Symmetric](https://github.com/GlitchedPolygons/Cryptography.Symmetric)
* [GlitchedPolygons.Services.Cryptography.Asymmetric](https://github.com/GlitchedPolygons/Cryptography.Asymmetric)

 <PackageReference Include="" Version="1.0.1" />
    <PackageReference Include="" Version="1.0.3" />
    <PackageReference Include="GlitchedPolygons.Services.Cryptography.Asymmetric" Version="1.0.0" />
    <PackageReference Include="GlitchedPolygons.Services.Cryptography.Symmetric" Version="1.0.0" />
    <PackageReference Include="Newtonsoft.Json" Version="12.0.1" />
    <PackageReference Include="RestSharp" Version="106.6.9" />

Reference this C# project inside your Glitched Epistle client solution.

API docs can be found here:
_[glitchedpolygons.github.io/GlitchedEpistle.Client](https://glitchedpolygons.github.io/GlitchedEpistle.Client/api/index.html)_
