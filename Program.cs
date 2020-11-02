using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

using VatsimLibrary.VatsimClient;
using VatsimLibrary.VatsimDb;

namespace hw
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"VatSimdbHelper.DATA_Dir)");
        
            using(var db = new VatsimDbContext()){

                Console.WriteLine("Which question do you want? (1-10) ");
                string answer = Console.ReadLine();
                if(answer == "1"){
                    Query1();
                }else if(answer == "2"){
                    Query2();
                }else if(answer == "3"){
                    Query3();
                }else if(answer == "4"){
                    Query4();
                }else if(answer == "5"){
                    Query5();
                }else if(answer == "6"){
                    Query6();
                }else if(answer == "7"){
                    Query7();
                }else if(answer == "8"){
                    Query8();
                }else if(answer == "9"){
                    Query9();
                }else{
                    Query10();
                }
            }
        }
            

                static void Query1()
                {
                    using(var db = new VatsimDbContext()){

                    // Query 1: Which pilot has been logged on the longest?*********************************************

                    var _pilots = db.Pilots.Select(p => p).ToList();

                    var TL = from p in _pilots select p.TimeLogon;
                    var tlMax = TL.Max();

                    var plength = 
                        from p in _pilots
                        where p.TimeLogon == tlMax
                        select p.Realname;

                    Console.WriteLine(plength.ToList()[0]);
                    }
                }
                 
                static void Query2()
                {
                    using(var db = new VatsimDbContext())
                    {

                    // Query 2: Which controller has been logged on the longest?***********************************************


                    var _controllers = db.Controllers.Select(p => p).ToList();

                    var TL = from p in _controllers select p.TimeLogon;
                    var tlMax = TL.Max();

                    var plength = 
                        from p in _controllers
                        where p.TimeLogon == tlMax
                        select p.Realname;

                    Console.WriteLine(plength.ToList()[0]);
                    }
                }

                static void Query3()
                {
                    using(var db = new VatsimDbContext()){

                    // Query 3: Which airport has the most departures?**************************************************

                    var _flights = db.Flights.Select(f => f).ToList();

                    var plannedDepAirports = 
                    from flight in _flights
                    group flight by flight.PlannedDepairport into flightGroup
                    select new 
                    {
                        Airport = flightGroup.Key,
                        CountOfDep = flightGroup.Count(),
                    };
                

                    List<int> countList = new List<int>();

                    foreach (var f in plannedDepAirports)
                    {
                        countList.Add(f.CountOfDep);
                    }

                    
                    var DepAirports = 
                    from highest in plannedDepAirports
                    where highest.CountOfDep == countList.Max()
                    select highest.Airport;

                    Console.WriteLine(DepAirports.ToList()[0]); 


                    Console.WriteLine();
                    }
                }

                static void Query4()
                {
                    using(var db = new VatsimDbContext()){

                    // Query 4: Which airport has the most arrivals?**************************************************

                    var _flights = db.Flights.Select(f => f).ToList();

                    var plannedDestAirports = 
                    from flight in _flights
                    group flight by flight.PlannedDestairport into flightGroup
                    select new 
                    {
                        Airport = flightGroup.Key,
                        CountOfDep = flightGroup.Count(),
                    };

                    List<int> countList = new List<int>();

                    foreach (var f in plannedDestAirports)
                    {
                        countList.Add(f.CountOfDep);
                    }

                    
                    var DestAirports = 
                    from highest in plannedDestAirports
                    where highest.CountOfDep == countList.Max()
                    select highest.Airport;

                    Console.WriteLine(DestAirports.ToList()[0]); 
                    }
                }

                static void Query5()
                {
                    using(var db = new VatsimDbContext()){

                    // Query 5: Who is flying at the highest altitude and what kind of plane are they flying?**************************************************

                    var _positions = db.Positions.Select(p => p).ToList();

                    var altitude = 
                    from position in _positions
                    select position.Altitude;

                    var altop = altitude.Max();

                    var pilotName =
                    from position in _positions
                    where position.Altitude == altop
                    select position.Realname;

                    var _flights = db.Flights.Select(f => f).ToList();

                    var pName = 
                    from flight in _flights
                    where flight.Realname == pilotName.ToList()[0]
                    select pilotName;

                    var plane =
                    from pn in pilotName
                    select pn.PlannedAircraft;

                    Console.WriteLine(altitude.ToList()[0]);
                    Console.WriteLine(pilotName.ToList()[0]);
                    Console.WriteLine(plane.ToList()[0]);


                    };
                }

                static void Query6()
                {
                    using(var db = new VatsimDbContext()){
                        // Query 6: Who is flying the slowest (hint: they can't be on the ground)************************************************* 

                        var _positions = db.Positions.Select(p => p).ToList();
                        var speed = 
                        from position in _positions
                        select position.Groundspeed;
                        var speedList =
                        from l in speed 
                        orderby Convert.ToInt32(l) ascending select l;

                        var lspeed = 
                        from a in speedList 
                        where a != "0" select a;
                        Console.WriteLine(lspeed.ToList()[0]);

                    }                       
                }

                static void Query7()
                {
                    using(var db = new VatsimDbContext()){

                    // Query 7: Which aircraft type is being used the most?**************************************************

                    var _flights = db.Flights.Select(p => p).ToList();
                    var plannedAircraft = 
                        from p in _flights 
                        group p by p.PlannedAircraft into g
                        select new 
                        {
                            Key = g.Key, Count = g.Count()
                        };

                    var result = plannedAircraft.Max(x => x.Count);
                    var cls = plannedAircraft.Where(x => x.Count == result).First();

                    Console.WriteLine("Class - {0} ", cls);
                    Console.WriteLine("Count - {0} ", result);
                    }
                }

                static void Query8()
                {
                    using(var db = new VatsimDbContext()){

                        // Query 8: Who is flying the fastest?**************************************************

                        var _positions = db.Positions.Select(p => p).ToList();

                        var speed =
                        from position in _positions 
                        select position.Groundspeed;

                        var listspeed =
                        from position in _positions
                        orderby Convert.ToInt32(position) descending select position;

                        Console.WriteLine(listspeed.ToList()[0]);     
                    }
                }

                static void Query9()
                {
                    using(var db = new VatsimDbContext()){

                        // Query 9: How many pilots are flying North? (270 degrees to 90 degrees)**************************************************
                        var _positions = db.Positions.Select(p => p).ToList();

                        var direction = 
                        from position in _positions
                        select position.Heading;

                        var North =
                        from a in direction
                        where 90 <= Convert.ToInt32(a) && Convert.ToInt32(a) <= 270 select a;

                        int Pilots = North.Count();

                        Console.WriteLine(Pilots);
                    }
                 
                }

                static void Query10()
                {
                    using(var db = new VatsimDbContext()){

                // Query 10: Which pilot has the longest remarks section of their flight?**************************************************

                var _flights = db.Flights.Select(p => p).ToList();

                var ThePilot = 
                from flight in _flights
                group flight by flight.PlannedRemarks into remarks
                select new
                {
                    Name = remarks.Key,
                    LongestRemark = remarks.Max(x => x.PlannedRemarks)
                };

                List<string> FindingLongest = new List<string>();
                foreach(var width in ThePilot){
                    FindingLongest.Add(width.LongestRemark);
                }

                string LR = FindingLongest.Max();

                var queryten = 
                from highest in ThePilot
                where highest.LongestRemark == LR
                select highest.Name;

                Console.WriteLine(queryten.ToList()[0]);
                }
                }
                       
        
    }
}
