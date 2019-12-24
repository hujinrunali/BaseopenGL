#version 330 core
out vec4 FragColor;
//定义材质结构体
struct Material{
	sampler2D diffuse;
	sampler2D specular;
	float shininess;
};

//定义定向光结构体
struct DirLight{
	vec3 direction;
	
	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
};

//定义点光源结构体
struct PointLight{
	vec3 position;
	
	float constant;
	float linear;
	float quadratic;
	
	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
};

//定义聚光光源
struct SpotLight{
	vec3 position;
	vec3 direction;//由光源指向物体
	
	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
	
	float constant;
	float linear;
	float quadratic;
	
	float cutOff;
	float outerCutOff;
};

//宏定义
//宏定义
#define NR_POINT_LIGHT 4

//输入变量
in vec3 FragPos;
in vec3 Normal;
in vec2 TexCoords;

//全局变量
uniform vec3 viewPos;
uniform Material material;
uniform DirLight dirLight;
uniform PointLight pointLights[NR_POINT_LIGHT];
uniform SpotLight spotLight;

//函数声明
vec3 CalcDirLight(DirLight light,vec3 Normal,vec3 viewDir);
vec3 CalcPointLight(PointLight light,vec3 Normal,vec3 viewDir,vec3 FragPos);
vec3 CalcSpotLight(SpotLight light,vec3 Normal,vec3 viewDir,vec3 FragPos);

void main()
{
	//属性
	vec3 norm = normalize(Normal);
	vec3 viewDir = normalize(viewPos-FragPos);
	//1.计算定向光
	vec3 result = CalcDirLight(dirLight,norm,viewDir);
	//2.计算点光源
	for(int i = 0 ;i < NR_POINT_LIGHT; ++i)
		result += CalcPointLight(pointLights[i],norm,viewDir,FragPos);
	//3.计算聚光源
	result += CalcSpotLight(spotLight,norm,viewDir,FragPos);
	
	FragColor = vec4(result,1.0);
}

//定点光计算函数定义，Normal已经被标准化
vec3 CalcDirLight(DirLight light,vec3 Normal,vec3 viewDir)
{
	//计算光源方向
	vec3 lightDir = normalize(-light.direction);
	//计算环境光
	vec3 ambient = light.ambient*texture(material.diffuse,TexCoords).rgb;
	//计算漫反射光
	float diff = max(dot(Normal,lightDir),0.0);
	vec3 diffuse = light.diffuse * diff * texture(material.diffuse,TexCoords).rgb;
	//计算镜面反射光
	vec3 reflectDir = reflect(-lightDir,Normal);
	float spec = pow(max(dot(viewDir,reflectDir),0.0),material.shininess);
	vec3 specular = light.specular*spec*texture(material.specular,TexCoords).rgb;
	
	//计算各个分量的总和
	vec3 result = ambient + diffuse + specular;
	return result;
}

//点光源光强计算函数
vec3 CalcPointLight(PointLight light,vec3 Normal,vec3 viewDir,vec3 FragPos)
{
	//计算光源方向，由片段方向指向光源
	vec3 lightDir = normalize(light.position - FragPos);
	//计算环境光
	vec3 ambient = light.ambient*texture(material.diffuse,TexCoords).rgb;
	//计算漫反射光
	float diff = max(dot(Normal,lightDir),0.0);
	vec3 diffuse = light.diffuse * diff * texture(material.diffuse,TexCoords).rgb;
	//计算镜面反射光
	vec3 reflectDir = reflect(-lightDir,Normal);
	float spec = pow(max(dot(viewDir,reflectDir),0.0),material.shininess);
	vec3 specular = light.specular*spec*texture(material.specular,TexCoords).rgb;
	
	//计算光的衰减
	float distance = length(light.position-FragPos);
	float attenuation = 1.0/(light.constant+light.linear*distance+light.quadratic*(distance*distance));
	
	//将各个分量乘以衰减
	//ambient *= attenuation;
	diffuse *= attenuation;
	specular *= attenuation;
	//计算各个分量的总和
	vec3 result = ambient + diffuse + specular;
	return result;
}

//聚光光源光强度计算函数
vec3 CalcSpotLight(SpotLight light,vec3 Normal,vec3 viewDir,vec3 FragPos)
{
	//计算光源方向，由片段方向指向光源
	vec3 lightDir = normalize(light.position - FragPos);
	//计算环境光
	vec3 ambient = light.ambient*texture(material.diffuse,TexCoords).rgb;
	//计算漫反射光
	float diff = max(dot(Normal,lightDir),0.0);
	vec3 diffuse = light.diffuse * diff * texture(material.diffuse,TexCoords).rgb;
	//计算镜面反射光
	vec3 reflectDir = reflect(-lightDir,Normal);
	float spec = pow(max(dot(viewDir,reflectDir),0.0),material.shininess);
	vec3 specular = light.specular*spec*texture(material.specular,TexCoords).rgb;
	
	//计算光的衰减
	float distance = length(light.position-FragPos);
	float attenuation = 1.0/(light.constant+light.linear*distance+light.quadratic*(distance*distance));
	
	//计算theta角度
	float theta = dot(lightDir,normalize(-light.direction));
	//就算圆锥外的衰减
	float epsilon = light.cutOff - light.outerCutOff;//角度越大，cos的值越小
	float intensity = clamp((theta-light.outerCutOff)/epsilon,0.0,1.0);
	
	//各个分量乘以衰减
	diffuse *= attenuation*intensity;
	specular *= attenuation*intensity;
		//计算各个分量的总和
	vec3 result = ambient + diffuse + specular;
	return result;
}

