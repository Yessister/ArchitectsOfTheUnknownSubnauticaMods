﻿using UnityEngine;
using SMLHelper.V2.Crafting;
using System.Collections;
using System.Collections.Generic;
using ArchitectsLibrary.Handlers;

namespace ArchitectsLibrary.Buildables
{
    class BuildableAlienRobot : GenericPrecursorDecoration
    {
        public BuildableAlienRobot() : base("BuildableAlienRobot", "Alien Robot", "An alien robot that wanders around. Placeable inside and outside.")
        {
        }

        protected override ConstructableSettings GetConstructableSettings => new ConstructableSettings(true, true, true, true, true, true, true, placeDefaultDistance: 2f, placeMinDistance: 2f, placeMaxDistance: 10f);

        protected override OrientedBounds[] GetBounds => new OrientedBounds[0];

        protected override string GetOriginalClassId => "4fae8fa4-0280-43bd-bcf1-f3cba97eed77";

        protected override bool ExteriorOnly => false;

        protected override TechData GetBlueprintRecipe()
        {
            return new TechData(new List<Ingredient>() { new Ingredient(TechType.PrecursorIonBattery, 1), new Ingredient(AUHandler.PrecursorAlloyIngotTechType, 1) });
        }

        protected override string GetSpriteName => "AlienRobot";
    }
}