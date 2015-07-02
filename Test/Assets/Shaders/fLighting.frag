#version 140

uniform mat4 uModel;

uniform sampler2D uTextureSampler;

uniform vec4 uColour;
uniform float uTextureFlag;

in vec2 oTexCoords;

out vec4 FragColour;

void main()
{
	vec4 texColour = (texture(uTextureSampler, oTexCoords) * uColour) * uTextureFlag;
	texColour += (1.0 - uTextureFlag) * uColour;
	//if (texColour.w <= 0.0) discard;
	FragColour = texColour;
}