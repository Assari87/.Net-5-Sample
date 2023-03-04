
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Core.Models.Proto
{
    public class ManpowerLogFactory
    {
        public static ManpowerLog GetManpowerLogEntity(FullManpowerLog logEntity, bool hasNote)
        {
            if (hasNote)
                return logEntity;
            else
                return new ManpowerLog
                {
                    CheckInDateTime = logEntity.CheckInDateTime,
                    CheckOutDateTime = logEntity.CheckOutDateTime,
                    ContactId = logEntity.ContactId,
                    NumWorkers = logEntity.NumWorkers,
                    ProtoSiteIntegrationConfig = logEntity.ProtoSiteIntegrationConfig,
                    UserId = logEntity.UserId,
                };
        }

        public static IntegratedManpowerLog GetIntegratedManpowerLogEntity(FullIntegratedManpowerLog logEntity, bool hasNote)
        {
            if (hasNote)
                return logEntity;
            else
                return new IntegratedManpowerLog()
                {
                    CheckInDateTime = logEntity.CheckInDateTime,
                    ContactId = logEntity.ContactId,
                    CheckOutDateTime = logEntity.CheckOutDateTime,
                    NumWorkers = logEntity.NumWorkers,
                    NumWorkersWithoutAccoumpaniers = logEntity.NumWorkersWithoutAccoumpaniers,
                    ProtoSiteIntegrationConfig = logEntity.ProtoSiteIntegrationConfig,
                    SiteAttendanceList = logEntity.SiteAttendanceList,
                    TotalWorkHours = logEntity.TotalWorkHours,
                    UserId = logEntity.UserId,
                };
        }
    }

    public interface IManpowerHasNote
    {
        public string Notes { get; set; }
    }
    public class ProtoManpowerLogRequest
    {
        [JsonProperty("manpower_log")]
        public ManpowerLog ManpowerLog { get; set; }

        public override string ToString()
        {
            return ManpowerLog?.ToString() ?? "NULL";
        }
    }

    public class FullManpowerLog : ManpowerLog, IManpowerHasNote
    {
        [JsonProperty("notes")]
        public virtual string Notes { get; set; }

        public override string ToString()
        {
            return base.ToString() + $"Notes:{Notes}";
        }
    }

    public class ManpowerLog : ProtoHasCustomFieldEntity
    {
        [JsonIgnore]
        public DateTimeOffset CheckInDateTime { get; set; }
        [JsonIgnore]
        public DateTimeOffset CheckOutDateTime { get; set; }

        [JsonProperty("date")]
        public string _Date => CheckInDateTime.ToString("yyyy-MM-dd");
        [JsonProperty("datetime")]
        public string _DateTime => CheckInDateTime.ToString("yyyy-MM-ddTHH\\:mm\\:ssZ");

        [JsonProperty("num_workers")]
        public int NumWorkers { get; set; }
        [JsonProperty("num_hours")]
        public virtual string _NumHours => (CheckOutDateTime - CheckInDateTime).TotalHours.ToString("f1");

        [JsonProperty("contact_id")]
        public string ContactId { get; set; }
        [JsonProperty("user_id")]
        public string UserId { get; set; }
        public string cost_code_id { get; }
        public string location_id { get; }
        public string trade_id { get; }


        public override string ToString()
        {
            return $"In:{CheckInDateTime}-Out:{CheckOutDateTime}-NumWorkers:{NumWorkers}-_NumHours:{_NumHours}";
        }
    }

    public class FullIntegratedManpowerLog : IntegratedManpowerLog, IManpowerHasNote
    {
        [JsonProperty("notes")]
        public virtual string Notes { get; set; }

        public override string ToString()
        {
            return base.ToString() + $"Notes:{Notes}";
        }
    }

    public class IntegratedManpowerLog : ManpowerLog
    {
        [JsonIgnore]
        public List<SiteAttendanceInfo> SiteAttendanceList { get; set; }
        [JsonIgnore]
        public int NumWorkersWithoutAccoumpaniers { get; set; }

        [JsonIgnore]
        public double TotalWorkHours { get; set; }

        [JsonProperty("num_hours")]
        public override string _NumHours => (TotalWorkHours / (NumWorkersWithoutAccoumpaniers <= 0 ? 1 : NumWorkersWithoutAccoumpaniers)).ToString("f1");


        public override string ToString()
        {
            return base.ToString() + $"SiteAttendanceIds:{string.Join(",", SiteAttendanceList?.Select(d => d.SiteAttendanceId) ?? new List<int>())}-_NumHours:{_NumHours}";
        }

    }
    public class SiteAttendanceInfo
    {
        public int SiteAttendanceId { get; set; }
        public string Mobile { get; set; }
    }
}
