TimeZoneDb
==========

**Description:**
A Time Zone Database for .net which uses real time IANA and CLDR data repositories as its data sources. Provides historical and current time zone data including mappings from IANA Time Zone Ids to Microsoft Time Zones Ids.

**Installation:**
The easiest way to install is by using [Nuget](http://nuget.org/packages/TimeZoneDb/)

**Documentation:** 
Documentation is available via the [Wiki](https://github.com/appease/TimeZoneDb/wiki)

**Quickstart:**

```C#
// initialize with default values
// Note: this should be done at startup and the returned instance should be used throughout your app
// preferrably this would be done by registering it as a singleton with your DI container. 
var timeZoneDbUseCases = new TimeZoneDbUseCases();

// get all time zones
var allTimeZones = timeZoneDbUseCases.GetAllTimeZones();

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
