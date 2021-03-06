﻿using BillingoApi.Core;
using BillingoApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BillingoApi
{
    public class Invoices
    {
        readonly Connection connection;

        public Invoices(string publicKey, string privateKey)
        {
            connection = new Connection(publicKey, privateKey);
        }

        /// <summary>
        /// Gets all the invoices async
        /// </summary>
        /// <param name="blockId">optional, the ID of the invoice block for filtering</param>
        /// <param name="fromInvoiceNo">optional, list all the invoices from this number, inclusive</param>
        /// <param name="limit">optional, limit of the results, defaults to 50</param>
        /// <param name="yearStart">optional, which year to list, defaults to current year</param>
        /// <param name="startDateAsString">optional, list invoices from this date, example format: "2018-01-01", defaults to "2010-01-01"</param>
        /// <param name="toInvoiceNo">optional, the final invoice number to return</param>
        /// <param name="yearEnd">optional, the final end year of invoice</param>
        /// <returns></returns>
        public async Task<List<Invoice>> QueryAsync(int fromInvoiceNo = -1,
                                                    long blockId = -1,
                                                    int limit = 50,
                                                    int yearStart = -1,
                                                    string startDateAsString = "2010-01-01",
                                                    int toInvoiceNo = -1,
                                                    int yearEnd = -1)
        {
            var l = new DataLoader<Invoice>(connection, "/invoices/query");

            if (yearStart == -1)
                yearStart = DateTimeOffset.Now.Year; // defaults to current Year

            var qp = l.QueryParameters;
            qp["start_date"] = startDateAsString; // if we don't specify this - it defaults to first day of current month
            qp["year_start"] = yearStart.ToString();

            if (blockId > -1)
                qp["block"] = blockId.ToString();

            if (fromInvoiceNo > -1)
                qp["num_start"] = fromInvoiceNo.ToString(); // fromInvoiceNo - inclusive - this will be the first invoice to return

            if (toInvoiceNo > -1)
                qp["num_end"] = toInvoiceNo.ToString();

            if (yearEnd > -1)
                qp["year_end"] = yearEnd.ToString();

            return await l.FetchAllAsync(limit);
        }

        public async Task<Invoice> FetchOneInvoice(long blockUid, int year, int invoiceNo)
        {
            var invoices = await QueryAsync(
                blockId: blockUid,
                fromInvoiceNo: invoiceNo,
                toInvoiceNo: invoiceNo,
                yearStart: year,
                yearEnd: year,
                limit: 1);

            if (invoices.Count != 1)
                return null;

            return invoices[0];
        }


    }
}
