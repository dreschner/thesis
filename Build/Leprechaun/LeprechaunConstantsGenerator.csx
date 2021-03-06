﻿// Generates Sitecore Habitat-style constants classes

Log.Debug($"Emitting constants templates for {ConfigurationName}...");

public string RenderFields(TemplateCodeGenerationMetadata template) {
    if (template.OwnFields.Length == 0) {
        return string.Empty;
    }

    var localCode = new System.Text.StringBuilder();

    localCode.Append(@"
            public struct Fields {
");

    foreach (var field in template.OwnFields) {
        localCode.AppendLine($@"
                /// <summary>
                /// Field type: {field.Type}
                /// Section: {field.Section}
                /// </summary>
                public const string {field.CodeName} = ""{FormatGuid(field.Id)}"";

                public const string {field.CodeName}FieldName = ""{field.Name}"";");
    }

    localCode.Append(@"
            }");

    return localCode.ToString();
}

public string RenderTemplates() {
    var localCode = new System.Text.StringBuilder();
	
    foreach(var template in Templates) {
        localCode.AppendLine($@"
        /// <summary>
        /// Full name: {NormalizeTemplateName(template.FullTypeName)}
        /// Path: {template.Path}
        /// </summary>
        public struct {NormalizeTemplateName(template.CodeName)} {{

            public const string Id = ""{FormatGuid(template.Id)}"";
            {RenderFields(template)}

        }}");
    }

    return localCode.ToString();
}

private string FormatGuid(System.Guid guid) {
    return guid.ToString("B").ToUpper();
}

private string NormalizeTemplateName(string name) {
    if (name.Contains("ParametersTemplate")) {
        return name.Replace("ParametersTemplate", string.Empty) + "RenderingParameters";
    }

    return name;
}

Code.AppendLine($@"//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.CodeDom.Compiler;

namespace {GenericRootNamespace} {{

    [GeneratedCode(""Leprechaun for {ConfigurationName}"", ""1.0.1"")]
    public struct Templates {{
        {RenderTemplates()}
    }}

}}");