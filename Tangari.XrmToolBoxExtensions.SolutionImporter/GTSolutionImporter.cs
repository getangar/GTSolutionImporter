﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace Tangari.XrmToolBoxExtensions.SolutionImporter
{
    [Export(typeof(IXrmToolBoxPlugin)),
    ExportMetadata("BackgroundColor", "MediumBlue"),
    ExportMetadata("PrimaryFontColor", "White"),
    ExportMetadata("SecondaryFontColor", "LightGray"),
    ExportMetadata("SmallImageBase64", "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"),
    ExportMetadata("BigImageBase64", "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"),
    ExportMetadata("Name", "GT Solution Importer"),
    ExportMetadata("Description", "A tool for importing a solution through multiple organizations by Gennaro Eduardo Tangari")]
    public class GTSolutionImporter : PluginBase
    {
        public override IXrmToolBoxPluginControl GetControl()
        {
            return new GTSolutionImporterControlPlugin();
        }
    }
}
