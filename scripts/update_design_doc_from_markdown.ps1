param(
  [string]$MarkdownPath = "docs\LETHE_망각의_군주_프로토타입_기획서_v0_11.md",
  [string]$DocxPath = "docs\LETHE_망각의_군주_완성형_기획서_v6.docx"
)

$ErrorActionPreference = "Stop"

function Escape-XmlText {
  param([string]$Text)
  return [System.Security.SecurityElement]::Escape($Text)
}

function New-ParagraphXml {
  param(
    [string]$Text,
    [string]$StyleId = "",
    [bool]$Bold = $false
  )

  $escaped = Escape-XmlText $Text
  $styleXml = ""
  if ($StyleId) {
    $styleXml = "<w:pPr><w:pStyle w:val=`"$StyleId`"/></w:pPr>"
  }
  $boldXml = ""
  if ($Bold) {
    $boldXml = "<w:rPr><w:b/></w:rPr>"
  }
  return "<w:p>$styleXml<w:r>$boldXml<w:t xml:space=`"preserve`">$escaped</w:t></w:r></w:p>"
}

function Convert-MarkdownLineToParagraphXml {
  param([string]$Line)

  if ($Line -match '^# (.+)$') {
    return New-ParagraphXml -Text $Matches[1] -StyleId "Title" -Bold $true
  }
  if ($Line -match '^## (.+)$') {
    return New-ParagraphXml -Text $Matches[1] -StyleId "Heading1" -Bold $true
  }
  if ($Line -match '^### (.+)$') {
    return New-ParagraphXml -Text $Matches[1] -StyleId "Heading2" -Bold $true
  }
  if ($Line -match '^- (.+)$') {
    return New-ParagraphXml -Text ("- " + $Matches[1])
  }
  if ($Line -match '^\d+\. (.+)$') {
    return New-ParagraphXml -Text $Line
  }
  if ($Line -match '^\|(.+)\|$') {
    return New-ParagraphXml -Text $Line
  }
  return New-ParagraphXml -Text $Line
}

if (!(Test-Path $MarkdownPath)) {
  throw "Markdown source not found: $MarkdownPath"
}
if (!(Test-Path $DocxPath)) {
  throw "DOCX target not found: $DocxPath"
}

Add-Type -AssemblyName System.IO.Compression
Add-Type -AssemblyName System.IO.Compression.FileSystem

$paragraphs = New-Object System.Collections.Generic.List[string]
$lines = Get-Content -LiteralPath $MarkdownPath -Encoding UTF8
foreach ($line in $lines) {
  if ([string]::IsNullOrWhiteSpace($line)) {
    continue
  }
  $paragraphs.Add((Convert-MarkdownLineToParagraphXml -Line $line))
}

$bodyXml = $paragraphs -join "`n"
$documentXml = @"
<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<w:document xmlns:wpc="http://schemas.microsoft.com/office/word/2010/wordprocessingCanvas" xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006" xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:r="http://schemas.openxmlformats.org/officeDocument/2006/relationships" xmlns:m="http://schemas.openxmlformats.org/officeDocument/2006/math" xmlns:v="urn:schemas-microsoft-com:vml" xmlns:wp14="http://schemas.microsoft.com/office/word/2010/wordprocessingDrawing" xmlns:wp="http://schemas.openxmlformats.org/drawingml/2006/wordprocessingDrawing" xmlns:w10="urn:schemas-microsoft-com:office:word" xmlns:w="http://schemas.openxmlformats.org/wordprocessingml/2006/main" xmlns:w14="http://schemas.microsoft.com/office/word/2010/wordml" xmlns:wpg="http://schemas.microsoft.com/office/word/2010/wordprocessingGroup" xmlns:wpi="http://schemas.microsoft.com/office/word/2010/wordprocessingInk" xmlns:wne="http://schemas.microsoft.com/office/word/2006/wordml" xmlns:wps="http://schemas.microsoft.com/office/word/2010/wordprocessingShape" mc:Ignorable="w14 wp14">
  <w:body>
$bodyXml
    <w:sectPr>
      <w:pgSz w:w="12240" w:h="15840"/>
      <w:pgMar w:top="1440" w:right="1440" w:bottom="1440" w:left="1440" w:header="720" w:footer="720" w:gutter="0"/>
      <w:cols w:space="720"/>
      <w:docGrid w:linePitch="360"/>
    </w:sectPr>
  </w:body>
</w:document>
"@

$archive = [System.IO.Compression.ZipFile]::Open($DocxPath, [System.IO.Compression.ZipArchiveMode]::Update)
try {
  $entry = $archive.GetEntry("word/document.xml")
  if ($entry) {
    $entry.Delete()
  }
  $newEntry = $archive.CreateEntry("word/document.xml", [System.IO.Compression.CompressionLevel]::Optimal)
  $stream = $newEntry.Open()
  try {
    $writer = New-Object System.IO.StreamWriter($stream, (New-Object System.Text.UTF8Encoding($false)))
    try {
      $writer.Write($documentXml)
    } finally {
      $writer.Dispose()
    }
  } finally {
    $stream.Dispose()
  }
} finally {
  $archive.Dispose()
}

Write-Output "Updated $DocxPath from $MarkdownPath"
