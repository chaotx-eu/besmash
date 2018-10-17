namespace RougeLikeDemo {
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Content;
    using System.Linq;
    using System;

    public class Dungeon0 : Map {
        // layout syntax: {{x, y, w[, h], solid(0|1).}*;}*
        public Dungeon0(string layout)
            : base("game/texture/sheets/map/dungeon0_sheet", new MapComponent[layout.Where(c => c == ';').Count()][])
        {
            int x = 0, y = 0, c;
            foreach(string row in layout.Split(';')) {
                c = row.Where(ch => ch == '.').Count();

                if(c > 0) {
                    Components[y] = new MapComponent[c];

                    foreach(string column in row.Split('.')) {
                        int[] values = column.Split(',').Where(s => !string.IsNullOrEmpty(s)).Select(n => int.Parse(n)).ToArray();
                        if(values.Count() == 4) Components[y][x++] = new MapComponent(values[0], values[1], values[2], values[3] != 0);
                        if(values.Count() == 5) Components[y][x++] = new MapComponent(values[0], values[1], values[2], values[3], values[4] != 0);
                    }

                    ++y;
                    x = 0;
                }
            }
        }

        // some demo layout
        public static string Layout {get; set;}

        // static Dungeon0() {
        //     Random rng = new Random();
        //     int Width = 30, Height = 20, style;

        //     for(int w, h = 0; h < Height; ++h) {
        //         for(w = 0; w < Width; ++w) {
        //             style = rng.Next(3)*16;

        //             if(w == 0 && h == 0) Layout += (48 + style) + ",0,16,1.";
        //             else if(w == 0 && h == Height-1) Layout += (48 + style) + ",16,16,1.";
        //             else if(w == Width-1 && h == 0) Layout += (48 + style) + ",32,16,1.";
        //             else if(w == Width-1 && h == Height-1) Layout += (48 + style) + ",48,16,1.";
        //             else if(w == 0 && h == Height/2) Layout += 96 + style + ",16,16,1.";
        //             else if(w == Width-1 && h == Height/2) Layout += 96 + style + ",48,16,1.";
        //             else if(w != 2 && h == Height/2) Layout += style + ",32,16,1.";
        //             else if(w == 0 || w == Width-1) Layout += style + ",16,16,1.";
        //             else if(h == 0 || h == Height-1) Layout += style + ",32,16,1.";
        //             else Layout += style + ",0,16,0.";
        //         }

        //         Layout += ";";
        //     }
        // }

        // public Dungeon0() : this(Layout) {}

        // spritesheet
        // public Dungeon0() : this(
        //       "0,0,16,0.  16,0,16,0.  32,0,16,0.  48,0,16,1.  64,0,16,1.  80,0,16,1.  96,0,16,1.  112,0,16,1.  128,0,16,1.;"
        //     + "0,16,16,1. 16,16,16,1. 32,16,16,1. 48,16,16,1. 64,16,16,1. 80,16,16,1. 96,16,16,1. 112,16,16,1. 128,16,16,1.;"
        //     + "0,32,16,1. 16,32,16,1. 32,32,16,1. 48,32,16,1. 64,32,16,1. 80,32,16,1. 96,32,16,1. 112,32,16,1. 128,32,16,1.;"
        //     + "0,48,16,1. 16,48,16,1. 32,48,16,1. 48,48,16,1. 64,48,16,1. 80,48,16,1. 96,48,16,1. 112,48,16,1. 128,48,16,1.;"
        // ) {}

        public Dungeon0()
            :  this("48,0,16,16,1.0,32,16,16,1.0,32,16,16,1.0,32,16,16,1.0,32,16,16,1.96,0,16,16,1.0,32,16,16,1.0,32,16,16,1.0,32,16,16,1.48,32,16,16,1.;16,16,16,16,1.0,0,16,16,0.0,0,16,16,0.0,0,16,16,0.0,0,16,16,0.16,16,16,16,1.0,0,16,16,0.0,0,16,16,0.0,0,16,16,0.16,16,16,16,1.;16,16,16,16,1.0,0,16,16,0.0,0,16,16,0.0,0,16,16,0.0,0,16,16,0.16,16,16,16,1.0,0,16,16,0.0,0,16,16,0.0,0,16,16,0.16,16,16,16,1.;16,16,16,16,1.0,0,16,16,0.0,0,16,16,0.0,0,16,16,0.0,0,16,16,0.16,16,16,16,1.0,0,16,16,0.0,0,16,16,0.0,0,16,16,0.16,16,16,16,1.;16,16,16,16,1.0,0,16,16,0.0,0,16,16,0.0,0,16,16,0.0,0,16,16,0.16,16,16,16,1.0,0,16,16,0.0,0,16,16,0.0,0,16,16,0.16,16,16,16,1.;96,16,16,16,1.0,32,16,16,1.0,32,16,16,1.0,0,16,16,0.0,32,16,16,1.16,48,16,16,1.0,32,16,16,1.0,0,16,16,0.0,32,16,16,1.96,48,16,16,1.;16,16,16,16,1.0,0,16,16,0.0,0,16,16,0.0,0,16,16,0.0,0,16,16,0.16,16,16,16,1.0,0,16,16,0.0,0,16,16,0.0,0,16,16,0.16,16,16,16,1.;16,16,16,16,1.0,0,16,16,0.0,0,16,16,0.0,0,16,16,0.0,0,16,16,0.0,0,16,16,0.0,0,16,16,0.0,0,16,16,0.0,0,16,16,0.16,16,16,16,1.;16,16,16,16,1.0,0,16,16,0.0,0,16,16,0.0,0,16,16,0.0,0,16,16,0.16,16,16,16,1.0,0,16,16,0.0,0,16,16,0.0,0,16,16,0.16,16,16,16,1.;48,16,16,16,1.0,32,16,16,1.0,32,16,16,1.0,32,16,16,1.0,32,16,16,1.128,32,16,16,1.0,32,16,16,1.0,32,16,16,1.0,32,16,16,1.48,48,16,16,1.;")
        {}
    }
}