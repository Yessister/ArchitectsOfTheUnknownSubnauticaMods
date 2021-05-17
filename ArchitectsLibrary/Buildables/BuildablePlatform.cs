﻿using UnityEngine;

namespace ArchitectsLibrary.Buildables
{
    class BuildablePlatform : GenericPrecursorDecoration
    {
        public BuildablePlatform() : base("BuildablePlatform", "Alien Platform", "A flat platform, for decoration.")
        {
        }

        protected override ConstructableSettings GetConstructableSettings => new ConstructableSettings(false, false, true, true, true, true, true, placeDefaultDistance: 4f, placeMinDistance: 2f, placeMaxDistance: 10f);

        protected override OrientedBounds[] GetBounds => new OrientedBounds[] { new OrientedBounds(new Vector3(0f, 0.5f, 0f), Quaternion.identity, new Vector3(2f, 0.4f, 2f)) };

        protected override string GetOriginalClassId => "738892ae-64b0-4240-953c-cea1d19ca111";

        protected override bool ExteriorProp => true;
    }
}
