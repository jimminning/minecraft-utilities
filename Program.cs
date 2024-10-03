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

		return new Point3d(x, y, z);
	}

	public static double GetUserDouble(string label)
	{
		Console.Write($"{label}: ");

		var input = Console.ReadLine();

		if (double.TryParse(input, out double inputDouble))
		{
			return inputDouble;
		}
		else
		{
			Console.WriteLine("Invalid input. Please only enter numbers.");
			return GetUserDouble(label);
		}
	}

	public static void ThreeD(double maxDistance, Point3d spawner1, Point3d spawner2)
	{
		var spawner1Points = GetSpawnerActivationPoints(maxDistance, spawner1);
		var spawner2Points = GetSpawnerActivationPoints(maxDistance, spawner2);
		var intersectingPoints = spawner1Points.FindAll(sp1 => spawner2Points.Exists(sp2 => sp2.X == sp1.X && sp2.Y == sp1.Y && sp2.Z == sp1.Z));
		intersectingPoints.Sort((s1, s2) => s1.Y.CompareTo(s2.Y) == 0 
												? s1.X.CompareTo(s2.X) == 0 
													? s1.Z.CompareTo(s2.Z) 
													: s1.X.CompareTo(s2.X) 
												: s1.Y.CompareTo(s2.Y));
		DisplayIntersectionPoints(intersectingPoints);
	}

	public static double DisplayIntersectionPointSummary(List<Point3d> intersectingPoints)
	{
		var mostY = intersectingPoints
							.GroupBy(x => x.Y)
							.Select(g => new { YCoordinate = g.Key, Count = g.Count() })
							.OrderByDescending(g => g.Count)
							.First();

		Console.WriteLine("Spawner Activation Zones Intersect!");
		Console.WriteLine($"Intersection Coordinate Count: {intersectingPoints.Count}");
		Console.WriteLine();
		Console.WriteLine($"Y Coordinate with most intersections:  [Y = {mostY.YCoordinate},  Count = {mostY.Count}]");
		Console.WriteLine();

		return mostY.YCoordinate;
	}

	public static void DisplayAbreviatedIntersectionPoints(List<Point3d> intersectingPoints, double yCoordinate)
	{
		var filteredPoints = intersectingPoints
				.Where(p => p.Y > (yCoordinate - 1) && p.Y < (yCoordinate + 1)).ToList();
		filteredPoints.Sort((s1, s2) => s1.Y.CompareTo(s2.Y) == 0 ? s1.X.CompareTo(s2.X) == 0 ? s1.Z.CompareTo(s2.Z) : s1.X.CompareTo(s2.X) : s1.Y.CompareTo(s2.Y));
		Console.WriteLine("Intersection of Spawner Activation Points:");
		Console.WriteLine();
		Console.WriteLine("X\tY\tZ");
		foreach (Point3d p in filteredPoints)
		{
			Console.WriteLine($"{p.X}\t{p.Y}\t{p.Z}");
		}
	}

	public static void DisplayAllIntersectionPoints(List<Point3d> intersectingPoints)
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

	public static void DisplayIntersectionPoints(List<Point3d> intersectingPoints)
	{
		Console.WriteLine();
		Console.WriteLine();

		if (intersectingPoints.Any())
		{
			var yCoor = DisplayIntersectionPointSummary(intersectingPoints);

			if (intersectingPoints.Count > 50)
			{
				DisplayAbreviatedIntersectionPoints(intersectingPoints, yCoor);
			}
			else 
			{
				DisplayAllIntersectionPoints(intersectingPoints);
			}

			
		}
		else
		{
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
			Console.WriteLine("X: " + p.X + "\tY:" + p.Y);
		}


	}

	public static bool InActivationZone(double maxDistance, Point3d spawner, Point3d testPoint)
	{
		// X² + Y² + Z² = r²
		var r2 = Math.Pow(maxDistance+0.5,2);
		var x2 = Math.Pow(Math.Abs(spawner.X - testPoint.X),2);
		var y2 = Math.Pow(Math.Abs(spawner.Y - testPoint.Y),2);
		var z2 = Math.Pow(Math.Abs(spawner.Z - testPoint.Z),2);

		// Buffer
		return Math.Floor(x2 + y2 + z2) <= Math.Floor(r2);

		// Actual
		// return (x2 + y2 + z2) <= r2;
	}

	public static List<Point3d> GetSpawnerActivationPoints(double maxDistance, Point3d spawnerPoint)
	{
		var spawnerActivationPoints = new List<Point3d>();
		for (double x = spawnerPoint.X - maxDistance; x <= (spawnerPoint.X + maxDistance); x++)
		{
			for (double y = spawnerPoint.Y - maxDistance; y <= (spawnerPoint.Y + maxDistance); y++)
			{
				for (double z = spawnerPoint.Z - maxDistance; z <= (spawnerPoint.Z + maxDistance); z++)
				{
					var testPoint = new Point3d(x,y,z);

					if (InActivationZone(maxDistance, spawnerPoint, testPoint))
					{
						spawnerActivationPoints.Add(testPoint);
					}
				}
			}
		}
		return spawnerActivationPoints;
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
					spawnerPoints.Add(new Point(x, y));
				}
			}
		}
		return spawnerPoints;
	}

	public class Point
	{
		public double X { get; set; }
		public double Y { get; set; }

		public Point(double x, double y)
		{
			X = x;
			Y = y;
		}
	}

	public class Point3d
	{
		public double X { get; set; }
		public double Y { get; set; }
		public double Z { get; set; }

		public Point3d(double x, double y, double z)
		{
			X = x;
			Y = y;
			Z = z;
		}
	}
}