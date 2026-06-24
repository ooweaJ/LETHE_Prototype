param(
    [string]$Root = ".",
    [string]$ConceptSheet = ""
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$project = Resolve-Path $Root
$sourceRoot = Join-Path $project "LETHE/Assets/_dev/Art/Source"
$mapRoot = Join-Path $project "LETHE/Assets/_dev/Art/Sprites/Map"

function Ensure-Dir([string]$path) {
    if (-not (Test-Path $path)) {
        New-Item -ItemType Directory -Path $path | Out-Null
    }
}

function ColorA([int]$a, [int]$r, [int]$g, [int]$b) {
    return [System.Drawing.Color]::FromArgb($a, $r, $g, $b)
}

function Brush([System.Drawing.Color]$c) {
    return New-Object System.Drawing.SolidBrush($c)
}

function PenC([System.Drawing.Color]$c, [float]$w) {
    $p = New-Object System.Drawing.Pen($c, $w)
    $p.StartCap = [System.Drawing.Drawing2D.LineCap]::Round
    $p.EndCap = [System.Drawing.Drawing2D.LineCap]::Round
    return $p
}

function New-Canvas([int]$w, [int]$h, [System.Drawing.Color]$clear) {
    $bmp = New-Object System.Drawing.Bitmap($w, $h, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $g = [System.Drawing.Graphics]::FromImage($bmp)
    $g.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
    $g.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
    $g.PixelOffsetMode = [System.Drawing.Drawing2D.PixelOffsetMode]::HighQuality
    $g.CompositingQuality = [System.Drawing.Drawing2D.CompositingQuality]::HighQuality
    $g.Clear($clear)
    return @{ Bitmap = $bmp; Graphics = $g }
}

function Save-Png($canvas, [string]$path) {
    Ensure-Dir (Split-Path $path)
    $canvas.Bitmap.Save($path, [System.Drawing.Imaging.ImageFormat]::Png)
    $canvas.Graphics.Dispose()
    $canvas.Bitmap.Dispose()
    Write-Host "Wrote $path"
}

function Draw-WaterVein($g, [System.Random]$rng, [int]$size, [int]$alpha) {
    $shadow = PenC (ColorA ([Math]::Min(180, $alpha + 38)) 3 10 14) 5.0
    $water = PenC (ColorA $alpha 48 165 178) 2.0
    $y = [float]$rng.Next(22, $size - 22)
    $g.DrawBezier($shadow, -12, $y, $size * 0.28, $y + $rng.Next(-40, 41), $size * 0.68, $y + $rng.Next(-38, 39), $size + 12, $y + $rng.Next(-24, 25))
    $g.DrawBezier($water, -12, $y, $size * 0.28, $y + $rng.Next(-40, 41), $size * 0.68, $y + $rng.Next(-38, 39), $size + 12, $y + $rng.Next(-24, 25))
    $shadow.Dispose()
    $water.Dispose()
}

function Draw-RootLine($g, [System.Random]$rng, [int]$size) {
    $root = PenC (ColorA 116 102 100 92) ([float]$rng.Next(2, 5))
    $x = [float]$rng.Next(-20, $size + 8)
    $y = [float]$rng.Next(0, $size)
    $g.DrawBezier($root, $x, $y, $x + $rng.Next(20, 80), $y + $rng.Next(-36, 37), $x + $rng.Next(76, 132), $y + $rng.Next(-28, 29), $x + $rng.Next(128, 220), $y + $rng.Next(-20, 21))
    $root.Dispose()
}

function Draw-FallbackTerrainTile([string]$name, [int]$variant) {
    $size = 256
    $baseColors = @(
        (ColorA 255 22 29 32),
        (ColorA 255 31 30 27),
        (ColorA 255 18 43 45),
        (ColorA 255 38 39 42),
        (ColorA 255 24 26 29),
        (ColorA 255 18 38 40),
        (ColorA 255 35 30 29),
        (ColorA 255 36 38 34)
    )
    $canvas = New-Canvas $size $size $baseColors[$variant % $baseColors.Length]
    $g = $canvas.Graphics
    $rng = New-Object System.Random (240624 + $variant * 251)

    for ($i = 0; $i -lt 80; $i++) {
        $a = $rng.Next(18, 54)
        $r = $rng.Next(34, 74)
        $brush = Brush (ColorA $a $r ($r + $rng.Next(-6, 11)) ($r + $rng.Next(-4, 14)))
        $x = [float]$rng.Next(-8, $size)
        $y = [float]$rng.Next(-8, $size)
        $w = [float]$rng.Next(4, 22)
        $h = [float]$rng.Next(3, 16)
        $g.FillEllipse($brush, $x, $y, $w, $h)
        $brush.Dispose()
    }

    if ($variant -in @(0, 2, 5)) {
        for ($i = 0; $i -lt 3; $i++) { Draw-WaterVein $g $rng $size (76 + $i * 12) }
    }

    if ($variant -in @(1, 5, 7)) {
        for ($i = 0; $i -lt 8; $i++) { Draw-RootLine $g $rng $size }
    }

    if ($variant -in @(3, 4)) {
        $crack = PenC (ColorA 108 6 10 14) 3.0
        for ($i = 0; $i -lt 13; $i++) {
            $x = [float]$rng.Next(0, $size)
            $y = [float]$rng.Next(0, $size)
            $g.DrawLine($crack, $x, $y, $x + $rng.Next(-70, 71), $y + $rng.Next(-44, 45))
        }
        $crack.Dispose()
    }

    if ($variant -eq 4) {
        for ($i = 0; $i -lt 16; $i++) {
            $brush = Brush (ColorA 96 104 146 146)
            $x = [float]$rng.Next(0, $size)
            $y = [float]$rng.Next(0, $size)
            $s = [float]$rng.Next(5, 15)
            $g.FillPolygon($brush, @(
                (New-Object System.Drawing.PointF($x, $y - $s)),
                (New-Object System.Drawing.PointF($x + $s * 0.8, $y)),
                (New-Object System.Drawing.PointF($x, $y + $s)),
                (New-Object System.Drawing.PointF($x - $s * 0.65, $y))
            ))
            $brush.Dispose()
        }
    }

    Save-Png $canvas (Join-Path $mapRoot $name)
}

function Crop-TerrainSheet([string]$sheetPath) {
    $sheet = [System.Drawing.Bitmap]::FromFile($sheetPath)
    try {
        $cols = 4
        $rows = 2
        $cellW = [int][Math]::Floor($sheet.Width / $cols)
        $cellH = [int][Math]::Floor($sheet.Height / $rows)
        $outSize = 256

        for ($i = 0; $i -lt 8; $i++) {
            $col = $i % $cols
            $row = [int][Math]::Floor($i / $cols)
            $x = $col * $cellW
            $y = $row * $cellH
            $w = if ($col -eq $cols - 1) { $sheet.Width - $x } else { $cellW }
            $h = if ($row -eq $rows - 1) { $sheet.Height - $y } else { $cellH }

            $canvas = New-Canvas $outSize $outSize (ColorA 255 18 24 27)
            $dest = New-Object System.Drawing.Rectangle 0, 0, $outSize, $outSize
            $src = New-Object System.Drawing.Rectangle $x, $y, $w, $h
            $canvas.Graphics.DrawImage($sheet, $dest, $src, [System.Drawing.GraphicsUnit]::Pixel)

            $overlay = Brush (ColorA 44 4 7 9)
            $canvas.Graphics.FillRectangle($overlay, 0, 0, $outSize, $outSize)
            $overlay.Dispose()

            $file = "tile_lethe_terrain_{0:D2}.png" -f ($i + 1)
            Save-Png $canvas (Join-Path $mapRoot $file)
            Copy-Item (Join-Path $mapRoot $file) (Join-Path $sourceRoot ($file -replace '\.png$', '_source.png')) -Force
        }

        $backdrop = New-Canvas 768 768 (ColorA 255 11 15 18)
        for ($i = 0; $i -lt 9; $i++) {
            $col = $i % 3
            $row = [int][Math]::Floor($i / 3)
            $tileIndex = ($i * 3) % 8
            $srcCol = $tileIndex % $cols
            $srcRow = [int][Math]::Floor($tileIndex / $cols)
            $src = New-Object System.Drawing.Rectangle ($srcCol * $cellW), ($srcRow * $cellH), $cellW, $cellH
            $dest = New-Object System.Drawing.Rectangle ($col * 256), ($row * 256), 256, 256
            $backdrop.Graphics.DrawImage($sheet, $dest, $src, [System.Drawing.GraphicsUnit]::Pixel)
        }
        $veil = Brush (ColorA 82 3 7 10)
        $backdrop.Graphics.FillRectangle($veil, 0, 0, 768, 768)
        $veil.Dispose()
        Save-Png $backdrop (Join-Path $mapRoot "spr_lethe_terrain_backdrop_01.png")
        Copy-Item (Join-Path $mapRoot "spr_lethe_terrain_backdrop_01.png") (Join-Path $sourceRoot "spr_lethe_terrain_backdrop_01_source.png") -Force
    }
    finally {
        $sheet.Dispose()
    }
}

Ensure-Dir $sourceRoot
Ensure-Dir $mapRoot

if ([string]::IsNullOrWhiteSpace($ConceptSheet)) {
    $ConceptSheet = Join-Path $sourceRoot "spr_lethe_terrain_sheet_01_source.png"
}

if (Test-Path $ConceptSheet) {
    Crop-TerrainSheet (Resolve-Path $ConceptSheet)
}
else {
    for ($i = 0; $i -lt 8; $i++) {
        Draw-FallbackTerrainTile ("tile_lethe_terrain_{0:D2}.png" -f ($i + 1)) $i
        $file = "tile_lethe_terrain_{0:D2}.png" -f ($i + 1)
        Copy-Item (Join-Path $mapRoot $file) (Join-Path $sourceRoot ($file -replace '\.png$', '_source.png')) -Force
    }
    Copy-Item (Join-Path $mapRoot "tile_lethe_terrain_03.png") (Join-Path $mapRoot "spr_lethe_terrain_backdrop_01.png") -Force
    Copy-Item (Join-Path $mapRoot "spr_lethe_terrain_backdrop_01.png") (Join-Path $sourceRoot "spr_lethe_terrain_backdrop_01_source.png") -Force
}
