using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Inflectra.SpiraTest.AddOns.QualityCenterImporter
{
    public static class Utils
    {
        public const string CUSTOM_FIELD_ALM_DEFECT_ID = "alm_defect_id";

        /// <summary>
        /// The various custom property types
        /// </summary>
        public enum CustomPropertyTypeEnum
        {
            Text = 1,
            Integer = 2,
            Decimal = 3,
            Boolean = 4,
            Date = 5,
            List = 6,
            MultiList = 7,
            User = 8
        }

        /// <summary>
        /// Test Set Status IDs
        /// </summary>
        public enum TestSetStatus
        {
            NotStarted = 1,
            InProgress = 2,
            Completed = 3,
            Blocked = 4,
            Deferred = 5
        }

        /// <summary>
        /// Artifact Types
        /// </summary>
        public enum ArtifactType
        {
            None = -1,
            Requirement = 1,
            TestCase = 2,
            Incident = 3,
            Release = 4,
            TestRun = 5,
            Task = 6,
            TestStep = 7,
            TestSet = 8,
            AutomationHost = 9
        }

        /// <summary>
        /// Importance / Priorities
        /// </summary>
        public enum RequirementImportanceEnum
        {
            None = -1,
            Critical = 1,
            High = 2,
            Medium = 3,
            Low = 4
        }

        /// <summary>
        /// Statuses
        /// </summary>
        public enum RequirementStatusEnum
        {
            Requested = 1,
            Planned = 2,
            InProgress = 3,
            Developed = 4,
            Accepted = 5,
            Rejected = 6,
            UnderReview = 7,
            Obsolete = 8,
            Tested = 9,
            Completed = 10
        }

        /// <summary>
        /// Types
        /// </summary>
        public enum RequirementTypeEnum
        {
            Package = -1,
            Need = 1,
            Feature = 2,
            UseCase = 3,
            UserStory = 4,
            Quality = 5
        }

        public static string SafeSubstring(this string input, int length)
        {
            if (input == null)
            {
                return "";
            }
            if (input.Length <= length)
            {
                return input;
            }
            return input.Substring(0, length);
        } 
    }
}
