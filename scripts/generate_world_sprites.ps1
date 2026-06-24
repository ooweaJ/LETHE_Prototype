param(
    [string]$Root = "."
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

function Draw-Tile([string]$name, [int]$variant) {
    $size = 192
    $baseColors = @(
        (ColorA 255 31 43 51),
        (ColorA 255 36 38 54),
        (ColorA 255 28 48 54),
        (ColorA 255 42 34 48)
    )
    $canvas = New-Canvas $size $size $baseColors[$variant]
    $g = $canvas.Graphics

    $rng = New-Object System.Random (120612 + $variant * 97)
    $edgePen = PenC (ColorA 68 120 178 190) 2.0
    $darkPen = PenC (ColorA 58 5 10 16) 3.0
    $glowPen = PenC (ColorA 92 54 216 218) 2.0
    $redPen = PenC (ColorA 72 164 45 58) 2.0

    for ($i = 0; $i -lt 11; $i++) {
        $x1 = [float]$rng.Next(4, $size - 4)
        $y1 = [float]$rng.Next(4, $size - 4)
        $x2 = [float]($x1 + $rng.Next(-72, 73))
        $y2 = [float]($y1 + $rng.Next(-36, 37))
        $pen = if (($i + $variant) % 4 -eq 0) { $glowPen } elseif ($i % 3 -eq 0) { $redPen } else { $darkPen }
        $g.DrawLine($pen, $x1, $y1, $x2, $y2)
    }

    for ($i = 0; $i -lt 5; $i++) {
        $x = [float]$rng.Next(-20, $size - 16)
        $y = [float]$rng.Next(-20, $size - 16)
        $w = [float]$rng.Next(58, 104)
        $h = [float]$rng.Next(28, 64)
        $brush = Brush (ColorA 28 78 100 112)
        $g.FillEllipse($brush, $x, $y, $w, $h)
        $brush.Dispose()
    }

    $g.DrawRectangle($edgePen, 3, 3, $size - 7, $size - 7)
    $g.DrawLine($edgePen, 0, $size * 0.50, $size, $size * 0.50)
    $g.DrawLine($edgePen, $size * 0.50, 0, $size * 0.50, $size)

    $edgePen.Dispose(); $darkPen.Dispose(); $glowPen.Dispose(); $redPen.Dispose()
    Save-Png $canvas (Join-Path $mapRoot $name)
}

function Draw-Backdrop {
    $w = 512
    $h = 512
    $canvas = New-Canvas $w $h (ColorA 255 12 18 25)
    $g = $canvas.Graphics
    $rng = New-Object System.Random 240624

    for ($i = 0; $i -lt 24; $i++) {
        $a = [int](18 + ($i % 5) * 6)
        $brush = Brush (ColorA $a 22 72 86)
        $x = [float]$rng.Next(-80, $w - 20)
        $y = [float]$rng.Next(-80, $h - 20)
        $rw = [float]$rng.Next(120, 260)
        $rh = [float]$rng.Next(44, 110)
        $g.FillEllipse($brush, $x, $y, $rw, $rh)
        $brush.Dispose()
    }

    $riverPen = PenC (ColorA 92 34 190 206) 5.0
    $shadowPen = PenC (ColorA 88 5 9 18) 9.0
    for ($i = 0; $i -lt 7; $i++) {
        $offset = $i * 34
        $g.DrawBezier($shadowPen, -30, 110 + $offset, 110, 70 + $offset, 280, 170 + $offset, 548, 112 + $offset)
        if ($i % 2 -eq 0) {
            $g.DrawBezier($riverPen, -24, 110 + $offset, 110, 70 + $offset, 280, 170 + $offset, 548, 112 + $offset)
        }
    }

    $runePen = PenC (ColorA 72 132 224 218) 2.0
    for ($i = 0; $i -lt 18; $i++) {
        $x = [float]$rng.Next(20, $w - 20)
        $y = [float]$rng.Next(20, $h - 20)
        $r = [float]$rng.Next(10, 28)
        $g.DrawEllipse($runePen, $x - $r, $y - $r, $r * 2, $r * 2)
        $g.DrawLine($runePen, $x - $r * 0.55, $y, $x + $r * 0.55, $y)
    }

    $riverPen.Dispose(); $shadowPen.Dispose(); $runePen.Dispose()
    Save-Png $canvas (Join-Path $mapRoot "spr_lethe_arena_backdrop_01.png")
}

Ensure-Dir $sourceRoot
Ensure-Dir $mapRoot

for ($i = 0; $i -lt 4; $i++) {
    $file = "tile_lethe_stone_0$($i + 1).png"
    Draw-Tile $file $i
    Copy-Item (Join-Path $mapRoot $file) (Join-Path $sourceRoot ($file -replace '\.png$', '_source.png')) -Force
}

Draw-Backdrop
Copy-Item (Join-Path $mapRoot "spr_lethe_arena_backdrop_01.png") (Join-Path $sourceRoot "spr_lethe_arena_backdrop_01_source.png") -Force
