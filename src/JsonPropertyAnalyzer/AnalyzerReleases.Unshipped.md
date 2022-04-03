; Unshipped analyzer release
; https://github.com/dotnet/roslyn-analyzers/blob/main/src/Microsoft.CodeAnalysis.Analyzers/ReleaseTrackingAnalyzers.Help.md

### New Rules

Rule ID | Category | Severity | Notes
--------|----------|----------|-------
JN01 | Naming | Info | Property is missing JsonPropertyNameAttribute (Newtonsoft.Json)
JN02 | Naming | Info | Property is missing JsonIgnoreAttribute (Newtonsoft.Json)
JN03 | Naming | Info | Class doesn't have JsonPropertyNameAttributes on all public properties (Newtonsoft.Json)
JS01 | Naming | Info | Property is missing JsonPropertyNameAttribute (System.Text.Json
JS02 | Naming | Info | Property is missing JsonIgnoreAttribute (System.Text.Json)
JS03 | Naming | Info | Class doesn't have JsonPropertyNameAttributes on all public properties (System.Text.Json)