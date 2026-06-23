param(
    [string]$Root = "."
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$project = Resolve-Path $Root
$sourceRoot = Join-Path $project "LETHE/Assets/_dev/Art/Source"
$spriteRoot = Join-Path $project "LETHE/Assets/_dev/Art/Sprites/Enemies"

function Ensure-Dir([string]$path) {
    if (-not (Test-Path $path)) {
        New-Item -ItemType Directory -Path $path | Out-Null
    }
}

function New-Bitmap([int]$w, [int]$h, [bool]$chroma) {
    $bmp = New-Object System.Drawing.Bitmap($w, $h, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $g = [System.Drawing.Graphics]::FromImage($bmp)
    $g.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
    $g.PixelOffsetMode = [System.Drawing.Drawing2D.PixelOffsetMode]::HighQuality
    $g.CompositingQuality = [System.Drawing.Drawing2D.CompositingQuality]::HighQuality
    if ($chroma) {
        $g.Clear([System.Drawing.Color]::FromArgb(255, 0, 255, 0))
    } else {
        $g.Clear([System.Drawing.Color]::Transparent)
    }
    return @{ Bitmap = $bmp; Graphics = $g }
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

function Points($pairs) {
    $pts = New-Object 'System.Drawing.PointF[]' ($pairs.Count)
    for ($i = 0; $i -lt $pairs.Count; $i++) {
        $pts[$i] = New-Object System.Drawing.PointF([float]$pairs[$i][0], [float]$pairs[$i][1])
    }
    return $pts
}

function Draw-Eye([System.Drawing.Graphics]$g, [float]$ox, [float]$oy, [int]$dir, [int]$frame) {
    $cx = $ox + 48
    $cy = $oy + 43 + (($frame % 2) * 1.5)
    $pupilShift = @((-6,0),(0,-4),(6,0),(0,4))[$dir]
    $outer = Brush (ColorA 190 132 72 210)
    $sclera = Brush (ColorA 225 205 235 255)
    $iris = Brush (ColorA 240 63 224 224)
    $dark = Brush (ColorA 245 16 18 28)
    $glowPen = PenC (ColorA 130 118 245 255) 3.0
    $g.FillEllipse($outer, $cx - 28, $cy - 22, 56, 44)
    $g.DrawEllipse($glowPen, $cx - 30, $cy - 24, 60, 48)
    $g.FillEllipse($sclera, $cx - 21, $cy - 15, 42, 30)
    $g.FillEllipse($iris, $cx - 10 + $pupilShift[0], $cy - 10 + $pupilShift[1], 20, 20)
    $g.FillEllipse($dark, $cx - 5 + $pupilShift[0], $cy - 5 + $pupilShift[1], 10, 10)
    for ($i = 0; $i -lt 5; $i++) {
        $x = $cx - 24 + $i * 12
        $tendril = PenC (ColorA 120 128 74 210) 3.5
        $g.DrawBezier($tendril, $x, $cy + 17, $x - 9, $cy + 32, $x + 8, $cy + 43, $x + (($i % 2) * 8 - 4), $cy + 54)
        $tendril.Dispose()
    }
    $outer.Dispose(); $sclera.Dispose(); $iris.Dispose(); $dark.Dispose(); $glowPen.Dispose()
}

function Draw-Splitter([System.Drawing.Graphics]$g, [float]$ox, [float]$oy, [int]$dir, [int]$frame) {
    $cx = $ox + 48
    $cy = $oy + 49
    $wobble = (($frame % 4) - 1.5) * 1.2
    $membrane = Brush (ColorA 180 255 184 70)
    $core = Brush (ColorA 230 255 234 120)
    $dark = Brush (ColorA 180 64 44 24)
    $pen = PenC (ColorA 170 255 226 126) 3.0
    $g.FillEllipse($membrane, $cx - 30 + $wobble, $cy - 21, 38, 42)
    $g.FillEllipse($membrane, $cx - 4 - $wobble, $cy - 23, 38, 46)
    $g.FillEllipse($dark, $cx - 14, $cy - 9, 28, 18)
    $g.DrawEllipse($pen, $cx - 33 + $wobble, $cy - 24, 42, 48)
    $g.DrawEllipse($pen, $cx - 7 - $wobble, $cy - 26, 44, 52)
    $g.FillEllipse($core, $cx - 8, $cy - 8, 16, 16)
    for ($i = 0; $i -lt 4; $i++) {
        $bubble = Brush (ColorA 120 255 236 150)
        $g.FillEllipse($bubble, $cx - 22 + $i * 13, $cy - 16 + (($i % 2) * 18), 7, 7)
        $bubble.Dispose()
    }
    $membrane.Dispose(); $core.Dispose(); $dark.Dispose(); $pen.Dispose()
}

function Draw-Priest([System.Drawing.Graphics]$g, [float]$ox, [float]$oy, [int]$dir, [int]$frame) {
    $cx = $ox + 48
    $cy = $oy + 50
    $robe = Brush (ColorA 220 72 86 88)
    $trim = PenC (ColorA 170 142 255 190) 3.0
    $void = Brush (ColorA 245 8 10 18)
    $rune = Brush (ColorA 220 64 255 156)
    $hood = Points @(
        @($cx, ($cy - 36)), @(($cx - 24), ($cy - 12)), @(($cx - 30), ($cy + 32)),
        @($cx, ($cy + 44)), @(($cx + 30), ($cy + 32)), @(($cx + 24), ($cy - 12))
    )
    $g.FillPolygon($robe, $hood)
    $g.DrawPolygon($trim, $hood)
    $g.FillEllipse($void, $cx - 12, $cy - 23, 24, 28)
    $g.FillEllipse($rune, $cx - 5, $cy - 9, 10, 10)
    $castPen = PenC (ColorA 170 74 255 170) 4.0
    $side = if ($dir -eq 0) { -1 } elseif ($dir -eq 2) { 1 } else { 0 }
    if ($side -eq 0) { $side = if (($frame % 2) -eq 0) { -1 } else { 1 } }
    $g.DrawLine($castPen, $cx + $side * 18, $cy + 2, $cx + $side * 34, $cy - 13)
    $g.FillEllipse($rune, $cx + $side * 30 - 6, $cy - 19, 12, 12)
    $robe.Dispose(); $trim.Dispose(); $void.Dispose(); $rune.Dispose(); $castPen.Dispose()
}

function Draw-Sheet([string]$name, [scriptblock]$draw) {
    $cols = 4; $rows = 8; $cell = 96
    $outDir = switch ($name) {
        "eye" { Join-Path $spriteRoot "Eye" }
        "splitter" { Join-Path $spriteRoot "Splitter" }
        "voidpriest" { Join-Path $spriteRoot "VoidPriest" }
    }
    Ensure-Dir $outDir
    $finalPath = switch ($name) {
        "eye" { Join-Path $outDir "sheet_enemy_eye_4dir.png" }
        "splitter" { Join-Path $outDir "sheet_enemy_splitter_4dir.png" }
        "voidpriest" { Join-Path $outDir "sheet_enemy_voidpriest_4dir.png" }
    }
    $sourcePath = switch ($name) {
        "eye" { Join-Path $sourceRoot "sheet_enemy_eye_4dir_chroma.png" }
        "splitter" { Join-Path $sourceRoot "sheet_enemy_splitter_4dir_chroma.png" }
        "voidpriest" { Join-Path $sourceRoot "sheet_enemy_voidpriest_4dir_chroma.png" }
    }

    foreach ($chroma in @($false, $true)) {
        $pair = New-Bitmap ($cols * $cell) ($rows * $cell) $chroma
        for ($r = 0; $r -lt $rows; $r++) {
            for ($c = 0; $c -lt $cols; $c++) {
                & $draw $pair.Graphics ($c * $cell) ($r * $cell) $c $r
            }
        }
        $path = if ($chroma) { $sourcePath } else { $finalPath }
        Ensure-Dir (Split-Path $path)
        $pair.Bitmap.Save($path, [System.Drawing.Imaging.ImageFormat]::Png)
        $pair.Graphics.Dispose()
        $pair.Bitmap.Dispose()
        Write-Host "Wrote $path"
    }
}

function Draw-Gatekeeper([System.Drawing.Graphics]$g, [bool]$chroma) {
    $cx = 96; $cy = 102
    $stone = Brush (ColorA 235 88 82 78)
    $iron = Brush (ColorA 235 42 45 50)
    $red = Brush (ColorA 240 255 66 52)
    $gold = Brush (ColorA 220 255 196 92)
    $void = Brush (ColorA 245 20 10 12)
    $outline = PenC (ColorA 210 255 76 64) 4.5
    $body = Points @(
        @(96, 22), @(55, 48), @(43, 118), @(65, 162),
        @(96, 174), @(127, 162), @(149, 118), @(137, 48)
    )
    $g.FillPolygon($stone, $body)
    $g.DrawPolygon($outline, $body)
    $g.FillRectangle($iron, 67, 62, 58, 74)
    $g.FillRectangle($void, 77, 73, 38, 30)
    $g.FillEllipse($red, 78, 78, 10, 10)
    $g.FillEllipse($red, 104, 78, 10, 10)
    $g.FillRectangle($gold, 91, 106, 10, 40)
    $g.FillRectangle($red, 82, 118, 28, 8)
    $leftHorn = Points @(@(57, 48), @(21, 30), @(45, 72))
    $rightHorn = Points @(@(135, 48), @(171, 30), @(147, 72))
    $g.FillPolygon($iron, $leftHorn)
    $g.FillPolygon($iron, $rightHorn)
    for ($i = 0; $i -lt 4; $i++) {
        $x = 48 + $i * 28
        $g.FillRectangle($red, $x, 151, 9, 9)
    }
    $stone.Dispose(); $iron.Dispose(); $red.Dispose(); $gold.Dispose(); $void.Dispose(); $outline.Dispose()
}

Ensure-Dir $sourceRoot
Draw-Sheet "eye" ${function:Draw-Eye}
Draw-Sheet "splitter" ${function:Draw-Splitter}
Draw-Sheet "voidpriest" ${function:Draw-Priest}

$bossDir = Join-Path $spriteRoot "Bosses"
Ensure-Dir $bossDir
foreach ($chroma in @($false, $true)) {
    $pair = New-Bitmap 192 192 $chroma
    Draw-Gatekeeper $pair.Graphics $chroma
    $path = if ($chroma) {
        Join-Path $sourceRoot "spr_boss_gatekeeper_01_chroma.png"
    } else {
        Join-Path $bossDir "spr_boss_gatekeeper_01.png"
    }
    $pair.Bitmap.Save($path, [System.Drawing.Imaging.ImageFormat]::Png)
    $pair.Graphics.Dispose()
    $pair.Bitmap.Dispose()
    Write-Host "Wrote $path"
}
