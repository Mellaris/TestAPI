using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using WebApplication1.Models;

namespace WebApplication1;

public partial class SensorsContext : DbContext
{
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    public SensorsContext()
    {

    }
    public SensorsContext(DbContextOptions<SensorsContext> options) : base(options)
    {

    }

    public DbSet<Sensor> Sensors { get; set; } = null!;
    public virtual DbSet<Measurement> Measurements { get; set; }

    public virtual DbSet<MeasurementsType> MeasurementsTypes { get; set; }

    public virtual DbSet<Meteostation> Meteostations { get; set; }

    public virtual DbSet<MeteostationsSensor> MeteostationsSensors { get; set; }
    public virtual DbSet<SensorsMeasurement> SensorsMeasurements { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
       => optionsBuilder.UseNpgsql("Host=193.176.78.35;Port=5433;Username=user2;Password=I6qrx");
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.UseIdentityColumns();

        modelBuilder.Entity<Measurement>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("measurements");

            entity.Property(e => e.measurement_ts)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("measurement_ts");
            entity.Property(e => e.measuremnet_type).HasColumnName("measurement_type");
            entity.Property(e => e.measurement_value)
                .HasPrecision(17, 2)
                .HasColumnName("measurement_value");
            entity.Property(e => e.sensor_inventory_number).HasColumnName("sensor_inventory_number");

            entity.HasOne(d => d.MeasurementTypeNavigation).WithMany()
                .HasForeignKey(d => d.measuremnet_type)
                .HasConstraintName("measurements_measurements_type_fk");

            entity.HasOne(d => d.SensorInventoryNumberNavigation).WithMany()
                .HasForeignKey(d => d.sensor_inventory_number)
                .HasConstraintName("measurements_meteostations_sensors_fk");
        });

        modelBuilder.Entity<MeasurementsType>(entity =>
        {
            entity.HasKey(e => e.type_id).HasName("measurements_type_id");

            entity.ToTable("measurements_type");

            entity.Property(e => e.type_id)
                .ValueGeneratedOnAdd()
                .HasColumnName("type_id");
            entity.Property(e => e.type_name)
                .HasMaxLength(31)
                .HasColumnName("type_name");
            entity.Property(e => e.type_units)
                .HasMaxLength(4)
                .IsFixedLength()
                .HasColumnName("type_units");
        });

        modelBuilder.Entity<Meteostation>(entity =>
        {
            entity.HasKey(e => e.station_id).HasName("meteostations_id");

            entity.ToTable("meteostations");

            entity.Property(e => e.station_id)
                .ValueGeneratedOnAdd()
                .HasColumnName("station_id");
            entity.Property(e => e.station_latitude)
                .HasPrecision(5, 2)
                .HasColumnName("station_latitude");
            entity.Property(e => e.station_longitude)
                .HasPrecision(5, 2)
                .HasColumnName("station_longitude");
            entity.Property(e => e.station_name)
                .HasMaxLength(127)
                .HasColumnName("station_name");
        });

        modelBuilder.Entity<MeteostationsSensor>(entity =>
        {
            entity.HasKey(e => e.sensor_inventory_number).HasName("meteostations_sensors_id");

            entity.ToTable("meteostations_sensors");

            entity.Property(e => e.sensor_inventory_number)
                .UseIdentityAlwaysColumn()
                .HasColumnName("sensor_inventory_number");
            entity.Property(e => e.added_ts)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("added_ts");
            entity.Property(e => e.removed_ts)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("removed_ts");
            entity.Property(e => e.sensor_id).HasColumnName("sensor_id");
            entity.Property(e => e.station_id).HasColumnName("station_id");

            entity.HasOne(d => d.Sensor);

            entity.HasOne(d => d.Station).WithMany(p => p.MeteostationsSensors)
                .HasForeignKey(d => d.station_id)
                .HasConstraintName("meteostations_sensors_meteostations_fk");
        });

        modelBuilder.Entity<Sensor>(entity =>
        {
            entity.HasKey(e => e.sensor_id).HasName("sensors_id");

            entity.ToTable("sensors");

            entity.Property(e => e.sensor_id)
                .ValueGeneratedOnAdd()
                .HasColumnName("sensor_id")
                ;

            entity.Property(e => e.sensor_name)
                .HasMaxLength(31)
                .HasColumnName("sensor_name");
        });

        modelBuilder.Entity<SensorsMeasurement>(entity =>
        {
            entity
                .HasKey(e => new { e.sensor_id, e.type_id });
            entity.ToTable("sensors_measurements");
            entity.Property(e => e.measurment_formula)
                .HasMaxLength(255)
                .HasColumnName("measurement_formula");
            entity.Property(e => e.sensor_id).HasColumnName("sensor_id");
            entity.Property(e => e.type_id).HasColumnName("type_id");

            entity.HasOne(d => d.Sensor).WithMany()
                .HasForeignKey(d => d.sensor_id)
                .HasConstraintName("sensors_measurements_sensors_fk");

            entity.HasOne(d => d.Type).WithMany()
                .HasForeignKey(d => d.type_id)
                .HasConstraintName("sensors_measurements_measurements_type_fk");
        });

        OnModelCreatingPartial(modelBuilder);

        //modelBuilder.Entity<Sensor>().ToTable("sensors").HasKey(sensor => sensor.sensor_id);

        //modelBuilder.Entity<Sensor>().Property(p => p.sensor_id).ValueGeneratedOnAdd();
    }

}