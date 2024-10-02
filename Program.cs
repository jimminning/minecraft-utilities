using System;
using System.Collections.Generic;
					
public class Program
{
	public static void Main()
	{
		var maxDistance = 15;
		// var spawner1 = new Point3d(-1083, 26, 1059);
		// var spawner2 = new Point3d(-1098, 37, 1063);
		
        Console.WriteLine("First Spawner");
        var spawner1 = GetSpawnerCoordinatesFromUser();
        Console.WriteLine();
        Console.WriteLine("Second Spawner");
		var spawner2 = GetSpawnerCoordinatesFromUser();
		
		ThreeD(maxDistance, spawner1, spawner2);
	}

    public static Point3d GetSpawnerCoordinatesFromUser()
    {
        Console.WriteLine("Enter spawner coordinates:");
        var x = GetUserDouble("X");
        var y = GetUserDouble("Y");
        var z = GetUserDouble("Z");

        return new Point3d(x,y,z);
    }

    public static double GetUserDouble(string label)
    {
        Console.Write($"{label}: ");

        var input = Console.ReadLine();

        if (double.TryParse(input, out double inputDouble))
        {
            return inputDouble;
        } else {
            Console.WriteLine("Invalid input. Please only enter numbers.");
            return GetUserDouble(label);
        }
    }
	
	public static void ThreeD(double maxDistance, Point3d spawner1, Point3d spawner2)
	{
		var spawner1Points = GetSpawnerPoints(maxDistance, spawner1);
		var spawner2Points = GetSpawnerPoints(maxDistance, spawner2);
		var intersectingPoints = spawner1Points.FindAll(sp1 => spawner2Points.Exists(sp2 => sp2.X == sp1.X && sp2.Y == sp1.Y && sp2.Z == sp1.Z));
		DisplayIntersectionPoints(intersectingPoints);
	}

    public static void DisplayIntersectionPoints(List<Point3d> intersectingPoints)
    {
        Console.WriteLine();
        Console.WriteLine();

        if (intersectingPoints.Any())
        {
            intersectingPoints.Sort((s1, s2) => s1.Y.CompareTo(s2.Y) == 0 ? s1.X.CompareTo(s2.X) == 0 ? s1.Z.CompareTo(s2.Z) : s1.X.CompareTo(s2.X) : s1.Y.CompareTo(s2.Y));
            Console.WriteLine("Intersection of Spawner Activation Points:");
            Console.WriteLine();
            Console.WriteLine("X\tY\tZ");
            foreach (Point3d p in intersectingPoints)
            {
                Console.WriteLine($"{p.X}\t{p.Y}\t{p.Z}");
            }
        }
        else {
            Console.WriteLine("Spawners have no intersecting activation points.");
        }
    }
	
	public static void TwoD(double maxDistance, Point spawner1, Point spawner2)
	{	
		var spawner1Points = GetSpawnerPoints(maxDistance, spawner1);
		var spawner2Points = GetSpawnerPoints(maxDistance, spawner2);
		var intersectingPoints = spawner1Points.FindAll(x => spawner2Points.Exists(y => y.X == x.X && y.Y == x.Y));
		
		foreach (Point p in intersectingPoints)
		{
			Console.WriteLine("X: " + p.X +  "\tY:" + p.Y);
		}
		
		
	}
	
	public static List<Point3d> GetSpawnerPoints(double maxDistance, Point3d spawnerPoint)
	{
		var spawnerPoints = new List<Point3d>();
		for (double x = spawnerPoint.X - maxDistance; x <= (spawnerPoint.X + maxDistance); x++)
		{
			for (double y = spawnerPoint.Y - maxDistance; y <= (spawnerPoint.Y + maxDistance); y++)
			{
				for (double z = spawnerPoint.Z - maxDistance; z <= (spawnerPoint.Z + maxDistance); z++)
				{
					var diffX = x - spawnerPoint.X;
					var diffY = y - spawnerPoint.Y;
					var diffZ = z - spawnerPoint.Z;

					if ((Math.Abs(diffX) + Math.Abs(diffY) + Math.Abs(diffZ)) <= maxDistance)
					{
						spawnerPoints.Add(new Point3d(x,y,z));
					}	
				}
			}
		}
		return spawnerPoints;
	}
	
	public static List<Point> GetSpawnerPoints(double maxDistance, Point spawnerPoint)
	{
		var spawnerPoints = new List<Point>();
		for (double x = spawnerPoint.X - maxDistance; x <= (spawnerPoint.X + maxDistance); x++)
		{
			for (double y = spawnerPoint.Y - maxDistance; y <= (spawnerPoint.Y + maxDistance); y++)
			{
				var diffX = x - spawnerPoint.X;
				var diffY = y - spawnerPoint.Y;

				if ((Math.Abs(diffX) + Math.Abs(diffY)) <= maxDistance)
				{
					spawnerPoints.Add(new Point(x,y));
				}
			}
		}
		return spawnerPoints;
	}
	
	public class Point
	{
		public double X {get; set;}
		public double Y {get; set;}
		
		public Point(double x, double y)
		{
			X = x;
			Y = y;
		}
	}
	
	public class Point3d
	{
		public double X {get; set;}
		public double Y {get; set;}
		public double Z {get; set;}
		
		public Point3d(double x, double y, double z)
		{
			X = x;
			Y = y;
			Z = z;
		}
	}
}