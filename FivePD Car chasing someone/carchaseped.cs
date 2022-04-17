using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using FivePD.API;
using FivePD.API.Utils;
using CitizenFX.Core.Native;

namespace CarChasePed
{
    [CalloutProperties("Car Chasing Person", "GGGDunlix", "0.1.2")]
    public class CarChasePed : Callout
    {
        Ped person, suspect;
        Vehicle car;
        
        

        public CarChasePed()
        {
            InitInfo(World.GetNextPositionOnStreet(Game.PlayerPed.GetOffsetPosition(Vector3Extension.Around(Game.PlayerPed.Position, 200f))));
            ShortName = "Car Chasing Someone";
            CalloutDescription = "Someone in a car is chasing someone on foot.";
            ResponseCode = 2;
            StartDistance = 60f;
        }

        public async override Task OnAccept()
        {

            InitBlip();
            UpdateData();
        }

        public async override void OnStart(Ped player)
        {
            base.OnStart(player);

            var cars = new[]
          {
               VehicleHash.Burrito,
               VehicleHash.Burrito2,
               VehicleHash.Burrito3,
               VehicleHash.Burrito4,
               VehicleHash.Burrito5,
               VehicleHash.GBurrito,
               VehicleHash.GBurrito2,
               VehicleHash.Bison,
               VehicleHash.Dubsta,
               VehicleHash.Dubsta2,
               VehicleHash.Dubsta3,
               VehicleHash.Taco
           };



            suspect = await SpawnPed(RandomUtils.GetRandomPed(), World.GetNextPositionOnStreet(Location.Around(10f)));
            person = await SpawnPed(RandomUtils.GetRandomPed(), World.GetNextPositionOnStreet(Location.Around(10f)));
            car = await SpawnVehicle(cars[RandomUtils.Random.Next(cars.Length)], suspect.Position + 2);
            suspect.AlwaysKeepTask = true;
            person.AlwaysKeepTask = true;
            suspect.BlockPermanentEvents = true;
            person.BlockPermanentEvents = true;


            suspect.SetIntoVehicle(car, VehicleSeat.Driver);
            suspect.Task.VehicleChase(person);
            person.Task.FleeFrom(suspect);
            // Blip color help was given by Natixco- Thanks bro :)

            Blip susblip = suspect.AttachBlip();
            susblip.Sprite = BlipSprite.Enemy;
            susblip.Color = BlipColor.Red;

            Blip personblip = person.AttachBlip();
            personblip.Sprite = BlipSprite.Player;
            personblip.Color = BlipColor.Blue;

            Blip carb = car.AttachBlip();
            carb.Sprite = BlipSprite.PersonalVehicleCar;
            carb.Color = BlipColor.Red;

        }
    }


}