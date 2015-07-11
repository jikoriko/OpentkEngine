#version 330

uniform mat4 uModel;
uniform vec4 uEyePosition;

uniform sampler2D uTextureSampler;
uniform float uTextureFlag;

uniform vec4 uColour;

struct LightProperties {
	bool On;
	int Type;
	vec4 Position;
	vec4 Direction;
	float SpotCutOff;
	vec3 AmbientLight;
	vec3 DiffuseLight;
	vec3 SpecularLight;
	float ConstantAttenuation;
	float LinearAttenuation;
	float QuadraticAttenuation;
	float Intensity;
};

uniform LightProperties uLight[5];

struct MaterialProperties {
	vec3 AmbientReflectivity;
	vec3 DiffuseReflectivity;
	vec3 SpecularReflectivity;
	float Shininess;
};

uniform MaterialProperties uMaterial;

uniform bool uLightingEnabled;

in vec4 oNormal;
in vec4 oSurfacePosition;
in vec2 oTexCoords;

out vec4 FragColour;

void ProcessLighting()
{
	vec4 texColour = (texture(uTextureSampler, oTexCoords) * uColour) * uTextureFlag;
	texColour += (1.0 - uTextureFlag) * uColour;
	if (texColour.w == 0.0) discard;

	FragColour = vec4(0.0, 0.0, 0.0, texColour.w);
	
	for (int i = 0; i < 5; i++)
	{
		if (!uLight[i].On) continue; 

		if (uLight[i].Type == 0)
		{
			//directional light
			vec3 lightDir = normalize(-uLight[i].Direction.xyz);
			vec3 normal = normalize(oNormal.xyz);

			float diffuseFactor = max(dot(normal, lightDir), 0.0);
			vec3 ambient = uLight[i].AmbientLight * uMaterial.AmbientReflectivity;
			vec3 colour = vec3(0.0);

			vec3 diffuse = uLight[i].DiffuseLight * uMaterial.DiffuseReflectivity * diffuseFactor;
			diffuse = (ambient + diffuse) * texColour.xyz;

			vec3 specular;
			if (diffuseFactor > 0.0)
			{
				vec3 eyeDirection = normalize(uEyePosition.xyz - oSurfacePosition.xyz);
				vec3 reflectedVector = reflect(lightDir, normal);
				float specularFactor = pow(max(dot(reflectedVector, eyeDirection), 0.0), uMaterial.Shininess);
			    specular = uLight[i].SpecularLight * uMaterial.SpecularReflectivity * specularFactor;
			}
			else
			{
				specular = vec3(0.0);
			}

			colour = diffuse + specular;
			colour *= uLight[i].Intensity;

			FragColour += vec4(colour, texColour.w);
		}
		else if (uLight[i].Type == 1)
		{
			//point light
			vec4 lightDir = normalize(uLight[i].Position - oSurfacePosition);
			
			float diffuseFactor = max(dot(oNormal, lightDir), 0);
			vec3 ambient = uLight[i].AmbientLight * uMaterial.AmbientReflectivity;
			vec3 colour = vec3(0.0);

			float dist = length(uLight[i].Position - oSurfacePosition);
			float att = 1.0 / (uLight[i].ConstantAttenuation + 
				(uLight[i].LinearAttenuation * dist) + 
				(uLight[i].QuadraticAttenuation * dist * dist));

			vec3 diffuse = uLight[i].DiffuseLight * uMaterial.DiffuseReflectivity * diffuseFactor;
			diffuse = (ambient + diffuse) * texColour.xyz;

			vec3 specular;
			if (diffuseFactor > 0.0)
			{
				vec4 eyeDirection = normalize(uEyePosition - oSurfacePosition);
				vec4 reflectedVector = reflect(-lightDir, oNormal);
				float specularFactor = pow(max(dot(reflectedVector, eyeDirection), 0.0), uMaterial.Shininess);
				specular = uLight[i].SpecularLight * uMaterial.AmbientReflectivity * specularFactor * texColour.xyz;
			}
			else
			{
				specular = vec3(0.0);
			}

			colour = diffuse * att + specular * att;
			colour *= uLight[i].Intensity;

			FragColour += vec4(colour, texColour.w);
		}
		else if (uLight[i].Type == 2)
		{
			//spot light
			vec4 lightDir = normalize(uLight[i].Position - oSurfacePosition);
			
			vec4 spotDir = normalize(uLight[i].Direction);
			float spotFactor = max(dot(-lightDir, spotDir), 0.0);

			float diffuseFactor = max(dot(oNormal, lightDir), 0.0);
	
			vec4 eyeDirection = normalize(uEyePosition - oSurfacePosition);
			vec4 reflectedVector = reflect(-spotDir, oNormal);

			float specularFactor = pow(max(dot(reflectedVector, eyeDirection), 0.0), uMaterial.Shininess);

			float dist = length(uLight[i].Position - oSurfacePosition);
			float att = 1.0 / (uLight[i].ConstantAttenuation + 
							(uLight[i].LinearAttenuation * dist) + 
							(uLight[i].QuadraticAttenuation * dist * dist));

			vec3 ambient = uLight[i].AmbientLight * uMaterial.AmbientReflectivity;
			vec3 colour = vec3(0.0);

			float outerCutOff = radians(uLight[i].SpotCutOff) - 0.05;

			if (spotFactor > radians(uLight[i].SpotCutOff))
			{
				
				
				vec3 diffuse = uLight[i].DiffuseLight * uMaterial.DiffuseReflectivity * diffuseFactor;
				diffuse = (ambient + diffuse) * texColour.xyz;

				vec3 specular;
				if (diffuseFactor > 0.0)
				{
					specular = uLight[i].SpecularLight * uMaterial.SpecularReflectivity * specularFactor * texColour.xyz;
				}
				else
				{
					specular = vec3(0.0);
				}
				
				colour = diffuse * att + specular * att;
				colour *= uLight[i].Intensity;
			}
			else if (spotFactor > outerCutOff)
			{
				float falloff = (spotFactor - outerCutOff) / (radians(uLight[i].SpotCutOff) - outerCutOff);
					
				vec3 diffuse = uLight[i].DiffuseLight * uMaterial.DiffuseReflectivity * diffuseFactor * falloff;
				diffuse = (ambient + diffuse) * texColour.xyz;

				vec3 specular;
				if (diffuseFactor > 0.0)
				{
					vec3 specular = uLight[i].SpecularLight * uMaterial.SpecularReflectivity * specularFactor * texColour.xyz * falloff;
				}
				else
				{
					specular = vec3(0.0);
				}
					
				colour = diffuse * att + specular * att;
				colour *= uLight[i].Intensity;
			}

			FragColour += vec4(colour, 1);
		}
	}
}

void main()
{

	if (uLightingEnabled)
	{
		ProcessLighting();
	}
	else
	{
		vec4 texColour = (texture(uTextureSampler, oTexCoords) * uColour) * uTextureFlag;
		texColour += (1.0 - uTextureFlag) * uColour;
		//if (texColour.w <= 0.0) discard;
		FragColour = texColour;
	}
}