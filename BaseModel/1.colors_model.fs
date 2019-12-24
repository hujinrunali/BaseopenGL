#version 330 core
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


out vec4 FragColor;

in vec2 TexCoords;
in vec3 FragPos;
in vec3 Normal;


uniform sampler2D texture_diffuse1;
uniform vec3 viewPos;
uniform PointLight pointLight;

vec3 CalcPointLight(PointLight light,vec3 Normal,vec3 viewDir,vec3 FragPos);
void main()
{   vec3 norm = normalize(Normal);
	vec3 viewDir = normalize(viewPos-FragPos);
	vec3 result = CalcPointLight(pointLight,norm,viewDir,FragPos);
    FragColor = vec4(result,1.0);
}

vec3 CalcPointLight(PointLight light,vec3 Normal,vec3 viewDir,vec3 FragPos)
{
	//1.计算光源的方向
	vec3 lightDir = normalize(light.position-FragPos);
	//2.计算环境光
	vec3 ambient = light.ambient * texture(texture_diffuse1,TexCoords).rgb;
	//3.计算漫反射光
	float diff = max(dot(Normal,lightDir),0.0);
	vec3 diffuse = light.diffuse*diff*texture(texture_diffuse1,TexCoords).rgb;
	//4.计算镜面反射光
	vec3 reflectDir = reflect(-lightDir,Normal);
	float spec = pow(max(dot(reflectDir,viewDir),0.0),32);
	vec3 specular = light.specular*spec*texture(texture_diffuse1,TexCoords).rgb;
	//5.计算衰减
	float distance = length(light.position-FragPos);
	float attenuation = 1.0/(light.constant+light.linear*distance+light.quadratic*(distance*distance));
	//6.计算结果
	diffuse *= attenuation;
	specular *= attenuation;
	
	return ambient+specular+diffuse;
}