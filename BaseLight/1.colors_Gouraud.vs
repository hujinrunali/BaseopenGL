#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;

out vec3 LightingColor;//输出光的颜色

uniform vec3 lightPos;
uniform vec3 viewPos;
uniform vec3 lightColor;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
	gl_Position = projection*view*model*vec4(aPos,1.0);
	
	//计算世界坐标位置和法向量
	vec3 Position = vec3(model*vec4(aPos,1.0));
	vec3 Normal = mat3(transpose(inverse(model)))*aNormal;

	//计算环境光
	float ambientStrength = 0.1;
	vec3 ambient = ambientStrength*lightColor;
	
	//计算漫反射光
	vec3 lightDir = normalize(lightPos-Position);
	vec3 norm = normalize(Normal);
	float diff = max(dot(norm,lightDir),0.0);
	vec3 diffuse = diff * lightColor;

	//计算镜面反射光
	float specularStrength = 1.0;
	vec3 viewDir = normalize(viewPos-Position);
	vec3 reflectDir = reflect(-lightDir,norm);
	float spec = pow(max(dot(viewDir,reflectDir),0.0),32);
	vec3 specular = specularStrength *spec*lightColor;
	
	//计算总的光照
	LightingColor = ambient + diffuse + specular;
}