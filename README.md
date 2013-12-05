TimeZoneDb
==========

**Description:**
Provides load, storage, and lookup of Time Zones.

**Installation:**
The easiest way to install is by using [Nuget](http://nuget.org/packages/TimeZoneDb/)

**Quickstart:**

```C#
// initialize with default values
// Note: this should be done at startup and the returned instance should be used throughout your app
// preferrably this would be done by registering it as a singleton with your DI container. 
var timeZoneDbUseCases = new TimeZoneDbUseCases();

// get all time zones
var allTimeZones = timeZoneDbUseCases = timeZoneDbUseCases.GetAllTimeZones();

// get a specific time zone by its IANA time zone id
var timeZone = timeZoneDbUseCases.GetTimeZoneWithIanaId("America/Los_Angeles");

// get the windows time zone name for a time zone
timeZone.MicrosoftId // Pacific Standard Time 

// get the ISO 3166-1 Alpha 2 code
timeZone.Iso31661Alpha2 // US

// get the coordinates
timeZone.Coordinates // {Latitude:340308.0, Longitude:-11814.0}

// get current moment in time
var moment = Moment.Create();

// get raw offset at current moment in time
timeZone.GetRawOffset(moment);

// get DST offset at current moment in time
timeZone.GetDstOffset(moment);

// get Abbreviation at current moment in time
timeZone.GetAbbreviation(moment)

```
