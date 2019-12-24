#version 330 core
out vec4 FragColor;

struct Material{
	sampler2D diffuse;
	sampler2D specular;
	float shininess;
};

struct Light{
	vec3 position;
	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
};


uniform Light light;
uniform Material material;
uniform vec3 viewPos;

in vec2 TexCoords;
in vec3 Normal;  
in vec3 FragPos;  
  


void main()
{
    //环境光的强度
    vec3 ambient = light.ambient*texture(material.diffuse,TexCoords).rgb;
  	
    // 漫反射光的强度 
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(light.position-FragPos);//由物体指向光源
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = light.diffuse * diff * texture(material.diffuse,TexCoords).rgb;
     
    //镜面反射光的强度
    vec3 viewDir = normalize(viewPos-FragPos);
    vec3 reflectDir = reflect(-lightDir,norm);//reflect要求第一个向量为从光源指向切片
    float spec = pow(max(dot(viewDir,reflectDir),0.0),material.shininess);
    vec3 specular = light.specular * spec*texture(material.specular,TexCoords).rgb;

    vec3 result = ambient+diffuse + specular;
    FragColor = vec4(result, 1.0);
} 