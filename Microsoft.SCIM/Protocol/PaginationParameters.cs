//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

namespace Microsoft.SCIM
{
    public class PaginationParameters : IPaginationParameters
    {
        private int? count;
        private int? startIndex;

        public int? Count
        {
            get => count;

            set
            {
                if (value.HasValue && value.Value < 0)
                {
                    count = 0;
                    return;
                }
                count = value;
            }
        }

        public int? StartIndex
        {
            get => startIndex;

            set
            {
                if (value.HasValue && value.Value < 1)
                {
                    startIndex = 1;
                    return;
                }
                startIndex = value;
            }
        }
    }
}