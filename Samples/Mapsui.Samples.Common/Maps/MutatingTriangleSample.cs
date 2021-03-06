﻿using System;
using BruTile.Predefined;
using Mapsui.Layers;
using Mapsui.Providers;
using System.Collections.Generic;
using Mapsui.Geometries;

namespace Mapsui.Samples.Common.Maps
{
    public static class MutatingTriangleSample
    {
        private static readonly Random Random = new Random(0);

        public static Map CreateMap()
        {
            var map = new Map();
            map.Layers.Add(CreateLayer());
            map.Layers.Add(CreateMutatingTriangleLayer(map.Envelope));
            return map;
        }

        private static ILayer CreateMutatingTriangleLayer(BoundingBox envelope)
        {   
            var layer = new MemoryLayer();
           
            var polygon = new Polygon(new LinearRing(GenerateRandomPoints(envelope, 3)));
            var feature = new Feature() { Geometry = polygon };
            var features = new Features();
            features.Add(feature);

            layer.DataSource = new MemoryProvider(features);

            PeriodicTask.Run(() =>
            {
                polygon.ExteriorRing = new LinearRing(GenerateRandomPoints(envelope, 3));
                // Clear cache for change to show
                feature.RenderedGeometry.Clear();
                // Trigger DataChanged notification
                layer.ViewChanged(true, layer.Envelope, 1);
            },
            TimeSpan.FromMilliseconds(1000));

            return layer;
        }

        public static IEnumerable<Point> GenerateRandomPoints(BoundingBox envelope, int count = 25)
        {
            var result = new List<Point>();

            for (var i = 0; i < count; i++)
            {
                result.Add(new Point(
                    Random.NextDouble() * envelope.Width + envelope.Left,
                    Random.NextDouble() * envelope.Height + envelope.Bottom));
            }

            result.Add(result[0]); // close polygon by adding start point.

            return result;
        }

        public static ILayer CreateLayer()
        {
            return new TileLayer(KnownTileSources.Create()) { Name = "OSM"};
        }
    }
}
