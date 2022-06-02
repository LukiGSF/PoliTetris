#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

/*matrix WorldViewProjection;

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float4 Color : COLOR0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

	output.Position = mul(input.Position, WorldViewProjection);
	output.Color = input.Color;

	return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
	return input.Color;
}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};*/





/*float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0
{
	float4 color = tex2D(s0, coords);
	color.gb = color.r;
	return color;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}*/

float amount;
Texture colorTex;

sampler colorSampler = sampler_state
{
	texture = <colorText>;
};

float4 GreyscalePixelShaderFunction(float2 textureCoordinate : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(colorSampler, textureCoordinate);
	float3 colrgb = color.rgb;
	float graycolor = dot(colrgb, float3(0.3, 0.59, 0.11));

	colrgb.rgb = lerp(dot(graycolor, float3(0.3, 0.59, 0.11)), colrgb, amount);

	return float4(colrgb.rgb, color.a);
}

technique Grayscale
{
	pass GrayscalePass
	{
		PixelShader = compile ps_2_0 GreyscalePixelShaderFunction();
	}
}