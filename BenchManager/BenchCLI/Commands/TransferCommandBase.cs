using System;
using System.Collections.Generic;
using System.Text;
using Mastersign.Sequence;
using static Mastersign.Sequence.Sequence;

namespace Mastersign.Bench.Cli.Commands
{
    abstract class TransferCommandBase : BenchCommand
    {
        private static readonly KeyValuePair<string, TransferPaths>[] SELECT_OPTION_PAIRS =
        {
            new KeyValuePair<string, TransferPaths>("SystemOnly", TransferPaths.System),
            new KeyValuePair<string, TransferPaths>("Config", TransferPaths.UserConfiguration),
            new KeyValuePair<string, TransferPaths>("Home", TransferPaths.HomeDirectory),
            new KeyValuePair<string, TransferPaths>("Projects", TransferPaths.ProjectDirectory),
            new KeyValuePair<string, TransferPaths>("AppLibs", TransferPaths.AppLibraries),
            new KeyValuePair<string, TransferPaths>("RequiredCache", TransferPaths.RequiredAppResourceCache),
            new KeyValuePair<string, TransferPaths>("Cache", TransferPaths.AppResourceCache),
            new KeyValuePair<string, TransferPaths>("RequiredApps", TransferPaths.RequiredApps),
            new KeyValuePair<string, TransferPaths>("Apps", TransferPaths.Apps),
            new KeyValuePair<string, TransferPaths>("All", TransferPaths.All),
        };

        protected static readonly string[] SELECT_OPTIONS 
            = Seq(SELECT_OPTION_PAIRS).Map(kvp => kvp.Key).ToArray();

        private static readonly IDictionary<string, TransferPaths> SELECT_OPTION_LOOKUP
            = Seq(SELECT_OPTION_PAIRS).ToDictionary(kvp => kvp.Key.ToLowerInvariant(), kvp => kvp.Value);

        private string[] ParseList(string value)
        {
            return string.IsNullOrEmpty(value)
                ? new string[0]
                : Seq(value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    .Map(p => p.Trim().ToLowerInvariant())
                    .ToArray();
        }

        protected bool IsTransferSelection(string value)
        {
            var options = Seq(SELECT_OPTIONS).Map(o => o.ToLowerInvariant());
            var parts = Seq(ParseList(value));
            return parts.All(p => options.Contains(p));
        }

        protected TransferPaths ParseTransferPaths(string value)
        {
            var options = Seq(SELECT_OPTIONS);
            var parts = Seq(ParseList(value));
            return parts
                .Map(p => SELECT_OPTION_LOOKUP[p])
                .Reduce((s, f) => s | f, TransferPaths.System);
        }
    }
}
