shader_type canvas_item;

uniform sampler2D screen_texture : hint_screen_texture, repeat_disable, filter_nearest;

// CRT Controls
uniform float curvature : hint_range(0.0, 10.0) = 3.0;
uniform float scanline_count : hint_range(0.0, 1080.0) = 300.0;
uniform float scanline_opacity : hint_range(0.0, 1.0) = 0.25;
uniform float vignette_opacity : hint_range(0.0, 1.0) = 0.5;

// Edge Static Controls
uniform float static_intensity : hint_range(0.0, 1.0) = 0.15;
uniform float static_edge_width : hint_range(0.0, 0.5) = 0.15;

// Deterministic pseudorandom noise generator based on UV and time
float random(vec2 uv) {
    return fract(sin(dot(uv, vec2(12.9898, 78.233) + TIME * 10.0)) * 43758.5453123);
}

// Lens distortion warp function
vec2 warp(vec2 uv) {
    vec2 delta = uv - 0.5;
    float dist = dot(delta, delta);
    return uv + delta * dist * (curvature / 10.0);
}

void fragment() {
    vec2 warped_uv = warp(SCREEN_UV);
    
    // Black out pixels outside the warped CRT screen frame
    if (warped_uv.x < 0.0 || warped_uv.x > 1.0 || warped_uv.y < 0.0 || warped_uv.y > 1.0) {
        COLOR = vec4(0.0, 0.0, 0.0, 1.0);
    } else {
        // 1. Sample base screen texture
        vec4 color = texture(screen_texture, warped_uv);
        
        // 2. Calculate edge mask for static (strongest at X=0 and X=1, fading toward center)
        float dist_from_edge = min(warped_uv.x, 1.0 - warped_uv.x);
        float edge_static_mask = 1.0 - smoothstep(0.0, static_edge_width, dist_from_edge);
        
        // 3. Generate static noise value and blend onto edges
        float noise = random(warped_uv);
        vec3 static_color = mix(color.rgb, vec3(noise), static_intensity);
        color.rgb = mix(color.rgb, static_color, edge_static_mask);
        
        // 4. Apply scanlines
        float scanline = sin(warped_uv.y * scanline_count * 3.14159 * 2.0);
        scanline = (scanline + 1.0) * 0.5;
        color.rgb -= scanline * scanline_opacity * color.rgb;
        
        // 5. Apply edge vignette
        float vignette = warped_uv.x * warped_uv.y * (1.0 - warped_uv.x) * (1.0 - warped_uv.y);
        vignette = clamp(pow(16.0 * vignette, 0.25), 0.0, 1.0);
        color.rgb *= mix(1.0 - vignette_opacity, 1.0, vignette);
        
        COLOR = color;
    }
}