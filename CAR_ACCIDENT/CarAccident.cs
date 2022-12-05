using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static CarAccident;
using Vector2 = System.Numerics.Vector2;
 
public class CarAccident : UnityEngine.MonoBehaviour
{
    public enum Area
    {
        Front,
        Rear,
        Side
    };
     public enum Vehicle_category
     {
         category_1 = 0,
         category_2 = 1,
         category_3 = 2,
         category_4 = 3,
         category_5 = 4,
         van = 5,
     };
    public enum SurfaceStatus
    {
        concrete_new, //!< Concrete (new)
        concrete_used, //!< Concrete (used)
        concrete_polished, //!< Concrete (polished by traffic)
        asphalt_new, //!< Asphalt (new)
        asphalt_used, //!< Asphalt (used)
        asphalt_polished, //!< Asphalt (polished by traffic)
        asphalt_excess_tar, //!< Asphalt (with excessive tar)
        paving_stone_new, //!< Paving stone (new)
        paving_stone_polished, //!< Paving stone (polished)
        compact_stone_new, //!< Compact stone (new)
        compact_stone_used, //!< Compact stone (polished by traffic)
        gravel_compacted, //!< Gravel (compacted)
        gravel_loose, //!< Gravel (loose)
        slag_compacted, //!< Slag (compacted)
        stone_crushed, //!< Stone (crushed)
        ice_smooth, //!< Ice (smooth)
        snow_compacted, //!< Snow (compacted)
        snow_loose, //!< Snow (loose)
        metal_grille_with_slots //!< Metal grille with slots
    }
    public enum Humidity
    {
        dry = 0,
        wet
    }
    public enum SpeedRange
    {
        minus_50 = 0, //Speed < 50 km/h
        plus_50 //Spedd > 50 km/h
    }


    [SerializeField] private int weight = 1215;//kg
    [SerializeField] private SurfaceStatus surfaceStatus;
    [SerializeField] private Humidity humidity;
    [SerializeField] private SpeedRange speedRange;
    [SerializeField] private Area area;
    [SerializeField] private Vehicle_category vehicle_category;


    [SerializeField] private double deformation_width = 1.7;//meters
    [SerializeField] private double C1 = 0.2;
    [SerializeField] private double C2 = 0.4;
    [SerializeField] private double C3 = 0.35;
    [SerializeField] private double C4 = 0.3;
    [SerializeField] private double C5 = 0.2;
    [SerializeField] private double C6 = 0.05;
    [SerializeField] private double L1 = 0.34;
    [SerializeField] private double L2 = 0.34;
    [SerializeField] private double L3 = 0.34;
    [SerializeField] private double L4 = 0.34;
    [SerializeField] private double L5 = 0.34;
   
    [SerializeField] private double roadway_slope=0.0;//degrees
    [SerializeField] private double collision_angle= 0.0;//degrees
    [SerializeField] private double skidmark_length = 40.0;//meters
    [SerializeField] private double crawl = 3.0;//distance travelled without skidmark in meters
    [SerializeField] private double BreakingEfficiency= 50.0;//Braking efficiency of the vehicle (If the vehicle has left four skid marks,
                                                             //the breaking efficiency is 100 percent. Subtract 20% for each front wheel that
                                                             //did not leave a skid mark and 30% for each back wheel that did not leave a skid mark).






    // Start is called before the first frame update
    void Start()
    {
        //double deformation_width = 1.7;
        // double weight = 1215;
        List<double> meassures = new List<double>() { C1, C2, C3, C4, C5, C6 };
        List<double> distances = new List<double>() { L1, L2, L3, L4, L5};
        List<double> distances2 = new List<double>() { 0, L1, L2, L3, L4, L5 };


        // List<double> meassures = new List<double>() { 0.2, 0.4, 0.35, 0.3, 0.2, 0.05 };
        // List<double> distances = new List<double>() { 0.34, 0.34, 0.34, 0.34, 0.34 };
        //List<double> distances2 = new List<double>() { 0, 0.34, 0.34, 0.34, 0.34, 0.34 };


        print("Speed collision:\n\n");
        print("Deformation width: ");
        print(deformation_width);
        print("\nCar weight: ");
        print(weight);
        print("\n");


        print("\nCompute Equivalent Barrier Speed: \n");
        Debug.Log("\n");
       /* Debug.Log("ID    Meassure    Distances");
        for (int i = 0; i < meassures.Count; i++)
        {
            print(i + "    " + meassures[i] + "    " + distances2[i]);
        }*/
        //Using McHenry's method 
        FrictionCoef frictionCoefficient = new FrictionCoef();


        double edef = energyDeformation(deformation_width, frictionCoefficient.TableCoeff[(int)vehicle_category].TryGetTuple(area).A, frictionCoefficient.TableCoeff[(int)vehicle_category].TryGetTuple(area).B, meassures, distances);
        //double edef = energyDeformation(deformation_width, frictionCoefficient.TableCoeff[1].Front.A, frictionCoefficient.TableCoeff[1].Front.B, meassures, distances);
        print("Energy Deformation using McHenry's method: " + edef);
        //print(frictionCoefficient.TableCoeff[(int)vehicle_category].TryGetTuple(area).A+"McHemry");
        //Energy correction factor in oblique collisions 
        edef = frictionCoefficient.frictionCoefficientSlopeCorrection(edef, collision_angle);
        print("Energy correction factor in oblique collisions : " + edef);


        //Using Prasad's method
        PrasadCoef PrasadCoefficient = new PrasadCoef();
        double prasadm = prasad(deformation_width, PrasadCoefficient.TablePrasadCoeff[(int)vehicle_category].TryGetTuple(area).d0, PrasadCoefficient.TablePrasadCoeff[(int)vehicle_category].TryGetTuple(area).d1, meassures, distances);      
        print("Energy Deformation using Prasad's method: " + prasadm);
        double prasadm2 = frictionCoefficient.frictionCoefficientSlopeCorrection(prasadm, collision_angle);
        //print(PrasadCoefficient.TablePrasadCoeff[1].Front.A + "  A, B " + PrasadCoefficient.TablePrasadCoeff[1].Front.B);


        // double ebs = equivalentBarrierSpeed(edef, weight);
        double ebs = equivalentBarrierSpeed(edef, weight);
        double ebs2 = equivalentBarrierSpeed(prasadm, weight);


        SpeedConvert converter = new SpeedConvert();
        double km_h = converter.convert(ebs, SpeedConvert.Units.MetrePerSecond, SpeedConvert.Units.KilometresPerHour);
        double km_h2 = converter.convert(ebs2, SpeedConvert.Units.MetrePerSecond, SpeedConvert.Units.KilometresPerHour);


        print("(McHenry's method)Equivalent Barrier Speed in m/s: " + ebs);
        print("(McHenry's method)Equivalent Barrier Speed in  km/h: " + km_h);
        print("(Prasad's method)Equivalent Barrier Speed in m/s: " + ebs2);
        print("(Prasad's method)Equivalent Barrier Speed in  km/h: " + km_h2);


        var coeff_range = frictionCoefficient.coefficientRange((FrictionCoef.SurfaceStatus)surfaceStatus, (FrictionCoef.Humidity)humidity, (FrictionCoef.SpeedRange)speedRange);
        //var coeff_range = frictionCoefficient.coefficientRange(FrictionCoef.SurfaceStatus.asphalt_polished, FrictionCoef.Humidity.dry, FrictionCoef.SpeedRange.plus_50);
       // print(coeff_range + "coef");


        /// EBS is the pre-collision speed
        
        //double m_s = speedGeneral(40., coeff_range.first, 0., ebs);
        print("Compute speed from skid marks:");
        double m_s = frictionCoefficient.speed3Phases(crawl, skidmark_length, coeff_range.Item1, .25, roadway_slope, ebs);
        double m_s2 = frictionCoefficient.speedGeneral(skidmark_length, coeff_range.Item1, roadway_slope, ebs);


        km_h = converter.convert(m_s, SpeedConvert.Units.MetrePerSecond, SpeedConvert.Units.KilometresPerHour);
        km_h2 = converter.convert(m_s2, SpeedConvert.Units.MetrePerSecond, SpeedConvert.Units.KilometresPerHour);


        print("\nInitial speed in m/s using speed3Phases(skidmarks): " + m_s);
        print("Initial speed in km/h using speed3Phases(skidmarks): " + km_h);
        print("\nInitial speed in m/s using speedGeneral(skidmarks): " + m_s2);
        print("Initial speed in km/h using speed3Phases(skidmarks): " + km_h2);
       
        double speed = Math.Sqrt(2.0f * DefineConstants.TIDOP_G * BreakingEfficiency/100 * skidmark_length * (coeff_range.Item1+(roadway_slope/100)));
        print("Calculate vehicle speed at the start of skidmarks m/s:" + speed);
        double km_h3 = converter.convert(speed, SpeedConvert.Units.MetrePerSecond, SpeedConvert.Units.KilometresPerHour);
        print("Calculate vehicle speed at the start of skidmarks km/h:" + km_h3);


    }


    // Update is called once per frame
    void Update()
    {


    }
    //Mc Hencry's energy deformation
    double energyDeformation(double deformationWidth,
                             int a,
                             int b,
                             List<double> measures,
                             List<double> distances)
    {
        int n = measures.Count;
        List<double> _distances = new List<double>();
        if (distances.Count == 0)
        {
            double d = deformationWidth / (n - 1.0);
            _distances = Enumerable.Repeat(d, n - 1).ToList();
        }
        else
        {
            _distances = new List<double>(distances);
        }


        if (b == 0)
        {
            return 0.0;
        }


        double aux = 0.0;


        for (int i = 1; i < n; i++)
        {
            double c1 = measures[i - 1];
            double c2 = measures[i];
            aux += _distances[i - 1] * ((a / 2.0) * (c1 + c2) + (b / 6.0) * (c1 * c1 + c1 * c2 + c2 * c2) + (int)a * a / (2.0 * b));
        }


        return aux;
    }
    //Prasad's energy deformation


    double prasad(double deformationWidth,
             double d0,
             double d1,
              List<double> measures,
              List<double> distances)
    {
        int n = measures.Count;
        List<double> _distances = new List<double>();
        if (distances.Count == 0)
        {
            double d = deformationWidth / (n - 1.0);
            _distances = Enumerable.Repeat(d, n - 1).ToList();
        }
        else
        {
            _distances = distances;
        }


        double aux = deformationWidth * d0 * d0 / 2.0;
        for (int i = 1; i < n; i++)
        {
            double c1 = measures[i - 1];
            double c2 = measures[i];
            double c2_c1 = c2 - c1;
            aux += _distances[i - 1] * (d0 * d1 * (c2_c1 / 2.0 + c1) + (d1 * d1 / 2.0) * (c2_c1 * c2_c1 / 3.0 + c1 * c1 + c2_c1 * c1));
        }


        return aux;
    }
    double equivalentBarrierSpeed(double edef,
                             double weight)
    {
        if (weight <= 0)
            return 0f;


        return Math.Sqrt(2.0f * edef / weight);
    }
}
public class SpeedConvert
{


    public enum Units
    {
        MetrePerSecond,
        KilometresPerHour,
        MilesPerHour,
        FootPerSecond
    }


    public SpeedConvert()
    {
    }
    public double[,] convFactorTableVel = new double[4, 4]
        {
                /*         m/s       km/h      mph        ft/s   */
                /*m/s*/   {1,       3.6,      2.236936,  3.280840},
                /*km/h*/  {0.277778, 1,       0.621371,  0.911344},
                /*mph*/   {0.44704,  1.609344, 1,        1.466667},
                /*ft/s*/  {0.3048,   1.09728,  0.681818,  1}
         };


    public string[] udNamesVel = new string[]
                {
             "m/s",
             "km/h",
              "mph",
              "fps"
                };


    public string[] udDescriptionVel = new string[]
                {
             "Metres per second",
             "Kilometres per hour",
             "Miles per hour",
             "Feet per second "
                };
    public void Dispose()
    {
    }
    public double convert(double speed, SpeedConvert.Units unitsIn, SpeedConvert.Units unitsOut)
    {
        if (unitsIn == unitsOut)
        {
            return speed;
        }
        if (speed == 0.0)
        {
            return speed;
        }


        return speed * convertFactor(unitsIn, unitsOut);
    }


    public double convertFactor(SpeedConvert.Units unitsIn, SpeedConvert.Units unitsOut)
    {
        int row = (int)unitsIn;
        int cols = (int)unitsOut;
        return convFactorTableVel[row, cols];
    }


    public string name(SpeedConvert.Units units)
    {
        return udNamesVel[(int)units];
    }


    public string description(SpeedConvert.Units units)
    {
        return udDescriptionVel[(int)units];
    }




    public int size()
    {
        return udNamesVel.Length;
    }
};


public class StiffnessCoefficient
{
    private Dictionary<CarAccident.Area, (int d0, int d1)> _areaToTuple = new Dictionary<CarAccident.Area, (int d0, int d1)>();
    public StiffnessCoefficient(
        Vector2 wheelBase,
        double track,
        double width,
        double length,
        double weigth,
        (int A, int B) front,
        (int A, int B) rear,
        (int A, int B) side)
    {
        WheelBase = wheelBase;
        Track = track;
        Width = width;
        Length = length;
        Weigth = weigth;
        _areaToTuple[CarAccident.Area.Front] = front;
        _areaToTuple[CarAccident.Area.Rear] = rear;
        _areaToTuple[CarAccident.Area.Side] = side;
    }
    public Vector2 WheelBase { get; }
    public double Track { get; }
    public double Width { get; }
    public double Length { get; }
    public double Weigth { get; }
    /* public (int A, int B) Front { get; }
     public (int A, int B) Rear { get; }
     public (int A, int B) Side { get; }*/
    public (int A, int B) TryGetTuple(CarAccident.Area area)
    {
        (int A, int B) tuple;
        if (_areaToTuple.TryGetValue(area, out tuple))
        {
            return tuple;
        }
        throw new System.Exception($"Cannot get tuple by type {area}...");
    }
};


public class FrictionCoef
{
    public enum SurfaceStatus
    {
        concrete_new, //!< Concrete (new)
        concrete_used, //!< Concrete (used)
        concrete_polished, //!< Concrete (polished by traffic)
        asphalt_new, //!< Asphalt (new)
        asphalt_used, //!< Asphalt (used)
        asphalt_polished, //!< Asphalt (polished by traffic)
        asphalt_excess_tar, //!< Asphalt (with excessive tar)
        paving_stone_new, //!< Paving stone (new)
        paving_stone_polished, //!< Paving stone (polished)
        compact_stone_new, //!< Compact stone (new)
        compact_stone_used, //!< Compact stone (polished by traffic)
        gravel_compacted, //!< Gravel (compacted)
        gravel_loose, //!< Gravel (loose)
        slag_compacted, //!< Slag (compacted)
        stone_crushed, //!< Stone (crushed)
        ice_smooth, //!< Ice (smooth)
        snow_compacted, //!< Snow (compacted)
        snow_loose, //!< Snow (loose)
        metal_grille_with_slots //!< Metal grille with slots
    }
    public enum SpeedRange
    {
        minus_50 = 0, //Speed < 50 km/h
        plus_50 //Spedd > 50 km/h
    }


    public enum Humidity
    {
        dry = 0,
        wet
    }


    public StiffnessCoefficient[] TableCoeff =
          {
             /* wheelbase   ,  track , length, width , weight,(frontA ,frontB),(rearA, rearB), ( sideA , sideB)  */
        new(new(2.055f, 2.408f), 1.298f, 4.059f, 1.544f, 990, (52850, 323830), (64050, 261820), (13475, 254930)),
        new (new(2.408f, 2.581f), 1.387f, 4.442f, 1.707f, 1380,( 45325, 296270),( 68425, 282490),(24500, 461630)),
        new (new(2.581f, 2.804f ), 1.496f, 4.983f, 1.844f, 1600, (55475, 385840),( 71750, 303160),(30275, 392730)),
        new (new(2.804f, 2.984f ), 1.57f, 5.405f, 1.956f, 1925, (62300, 234260), (62475, 389570), (25025, 344500)),
        new (new(2.984f, 3.129f ), 1.618f, 5.574f, 2.017f, 2300, (56875, 254930), (51975, 482300), (30975, 323830)),
        new (new(2.769f, 3.302f ), 1.717f, 4.633f, 1.981f, 1970, (67025, 868140), (52500, 378950),(0, 0) )
         };


    public double[,,,] coeff = new double[19, 2, 2, 2] {
          { {{0.80, 1.20}, {0.70, 1.0} },  {{0.5,  0.8 }, {0.4,  0.75}} },
          { {{0.6,  0.8 }, {0.6,  0.75}},  {{0.45, 0.7 }, {0.45, 0.65}} },
          { {{0.55, 0.75}, {0.50, 0.65}},  {{0.45, 0.65}, {0.45, 0.6} } },
          { {{0.8,  1.2 }, {0.65, 1.0} },  {{0.5,  0.8 }, {0.45, 0.75}} },
          { {{0.6,  0.8 }, {0.55, 0.7} },  {{0.45, 0.7 }, {0.4,  0.65}} },
          { {{0.55, 0.75}, {0.45, 0.65}},  {{0.45, 0.65}, {0.4,  0.6} } },
          { {{0.5,  0.6 }, {0.35, 0.6} },  {{0.3,  0.6 }, {0.25, 0.55}} },
          { {{0.75, 0.95}, {0.6,  0.85}},  {{0.5,  0.75}, {0.45, 0.7} } },
          { {{0.6,  0.8 }, {0.55, 0.75}},  {{0.4,  0.7 }, {0.4,  0.6} } },
          { {{0.75, 1.0 }, {0.7,  0.9} },  {{0.65, 0.9 }, {0.6,  0.85}} },
          { {{0.5,  0.7 }, {0.45, 0.65}},  {{0.3,  0.5 }, {0.25, 0.5} } },
          { {{0.55, 0.85}, {0.5,  0.8} },  {{0.4,  0.8 }, {0.4,  0.6} } },
          { {{0.4,  0.7 }, {0.4,  0.7} },  {{0.45, 0.75}, {0.45, 0.75}} },
          { {{0.5,  0.7 }, {0.5,  0.7} },  {{0.65, 0.75}, {0.65, 0.75}} },
          { {{0.55, 0.75}, {0.55, 0.75}},  {{0.55, 0.75}, {0.55, 0.75}} },
          { {{0.1,  0.25}, {0.07, 0.2} },  {{0.05, 0.1 }, {0.05, 0.1} } },
          { {{0.3,  0.55}, {0.35, 0.55}},  {{0.3,  0.6 }, {0.3,  0.6} } },
          { {{0.1,  0.25}, {0.1,  0.2} },  {{0.3,  0.6 }, {0.3,  0.6} } },
          { {{0.7,  0.9 }, {0.35, 0.75}},  {{0.25, 0.45}, {0.2,  0.35}} }
        };


    public double frictionCoefficientSlopeCorrection(double frictionCoefficient,
                                              double slope)
    {
        double angle = slope * DefineConstants.TIDOP_DEG_TO_RAD;
        return frictionCoefficient * Math.Cos(angle) + Math.Sin(angle);
    }


    public double speedGeneral(double length,
                        double frictionCoefficient,
                        double slope,
                        double endSpeed)
    {
        return Math.Sqrt(endSpeed * endSpeed + 2.0 *
               frictionCoefficientSlopeCorrection(frictionCoefficient, slope) * DefineConstants.TIDOP_G * length);
    }


    public double speed3Phases(double x,
                        double length,
                        double frictionCoefficient,
                        double responseTime,
                        double slope,
                        double endSpeed)
    {
        double _nu = frictionCoefficientSlopeCorrection(frictionCoefficient, slope);
        return 0.5 * _nu * DefineConstants.TIDOP_G * responseTime + Math.Sqrt(endSpeed * endSpeed + 2 * _nu * DefineConstants.TIDOP_G * (x + length));
    }


    public double stopDistanceGeneral(double speed,
                               double frictionCoefficient,
                               double reactionTime,
                               double slope)
    {
        return speed * reactionTime + (speed * speed) / (2 * frictionCoefficientSlopeCorrection(frictionCoefficient, slope) * DefineConstants.TIDOP_G);
    }


    public double stopDistance3Phases(double speed,
                               double frictionCoefficient,
                               double reactionTime,
                               double responseTime,
                               double slope)
    {
        double nu = frictionCoefficientSlopeCorrection(frictionCoefficient, slope);
        double aux = (speed - (nu * DefineConstants.TIDOP_G * responseTime) / 2.0);
        return speed * (reactionTime + responseTime) - (nu * DefineConstants.TIDOP_G * responseTime * responseTime) / 4.0 + aux * aux / (2 * nu * DefineConstants.TIDOP_G);
    }
    public (double min, double max) coefficientRange(SurfaceStatus surfaceStatus, Humidity humidity, SpeedRange range)
    {
        double min = coeff[(int)surfaceStatus, (int)humidity, (int)range, 0];
        double max = coeff[(int)surfaceStatus, (int)humidity, (int)range, 1];
        return (min, max);
    }
};
public class PrasadCoefficient 
{
    /*  public PrasadCoefficient(
         (double d0, double d1) front,
         (double d0, double d1) rear,
         (double d0, double d1) side)
      {
          Front = front;
          Rear = rear;
          Side = side;
      }
      public (double d0, double d1) Front { get; }
      public (double d0, double d1) Rear { get; }
      public (double d0, double d1) Side { get; }
    };*/
    readonly Dictionary<CarAccident.Area, (double d0, double d1)> _areaToTuple = new Dictionary<CarAccident.Area, (double d0, double d1)>();


    public PrasadCoefficient(
       (double d0, double d1) front,
       (double d0, double d1) rear,
       (double d0, double d1) side)
    {
        _areaToTuple[CarAccident.Area.Front] = front;
        _areaToTuple[CarAccident.Area.Rear] = rear;
        _areaToTuple[CarAccident.Area.Side] = side;
    }
    public (double d0, double d1) TryGetTuple(CarAccident.Area area)
    {
        (double d0, double d1) tuple;
        if (_areaToTuple.TryGetValue(area, out tuple))
        {
            return tuple;
        }
        throw new System.Exception($"Cannot get tuple by type {area}...");
    }


};
public class PrasadCoef
{ 
    public PrasadCoefficient[] TablePrasadCoeff =  
    {
             /* _______________________________________________
              |    Front       |     Rear      |     Side     |
              |----------------|---------------|--------------|
              | d0    |  d1    |  d0   |  d1   |  d0  | d1    |
              |-----------------------------------------------|*/
        new( (92.87,  569.06), (125.18, 511.68), (26.69, 504.9  )),
        new( (83.27,  544.31), (128.74, 531.50), (36.06, 679.43 )),
        new( (89.31,  621.16), (130.31, 550.6),  (48.31, 626.68 )),
        new( (128.68, 484.16), (100.09, 624.16), (42.64, 586.94 )),
        new( (112.64, 504.91), (74.84,  694.48), (54.43, 569.06 )),
        new( (71.94,  931.74), (85.28,  615.5),  (0.0,    0.0   ))
    };
};
public static class DefineConstants
{
    public const double TIDOP_PI_2 = 1.5707963267948966192313216916398;
    public const double TIDOP_PI = 3.1415926535897932384626433832795;
    public const double TIDOP_2PI = 6.283185307179586476925286766559;
    public const double TIDOP_RAD_TO_DEG = 57.295779513082320876798154814105;
    public const double TIDOP_DEG_TO_RAD = 0.01745329251994329576923690768489;
    public const double TIDOP_RAD_TO_GRAD = 63.661977236758134307553505349006;
    public const double TIDOP_GRAD_TO_RAD = 0.0157079632679489661923132169164;
    public const double TIDOP_G = 9.81; // Gravity
};
