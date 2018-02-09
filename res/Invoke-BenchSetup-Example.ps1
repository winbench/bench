.\Invoke-BenchSetup.ps1 `
    -TargetDir ".\bench_mastersign" `
    -Configuration @{
        "AppLibs" = @{
            "core" = "github:mastersign/bench-apps-core"
            "mastersign" = "github:mastersign/bench-apps-mastersign"
        }
    } `
    -ActivatedApps @(
        "Mastersign.DynamicNodes"
        "Mastersign.MapMap"
        "Mastersign.WinMan"
        "Mastersign.DisplayManager"
        "Mastersign.HtmlDisplay"
    )
