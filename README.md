# Sistema veterinario - Console

Sistema de gestão para clínica veterinária desenvolvido em C# para console.

## Estrutura do Projeto

- **Classes de Entidade**: 15 classes representando todas as entidades do sistema (Tutor, Paciente, veterinario, etc.)
- **CRUDs**: Operações de Create, Read, Update e Delete para cada entidade principal
- **Menus por Perfil**: 3 perfis de usuario com menus específicos (Recepcionista, veterinario, Gerente)

## Como Executar

### Compilar todos os arquivos:

```bash
csc *.cs
```

### Executar:

```bash
./Program.exe
```

## Funcionalidades

### Perfil: Recepcionista
- Gerenciar Tutores
- Gerenciar Pacientes
- Gerenciar Agendamentos

### Perfil: veterinario
- Consultar Agendamentos
- Gerenciar Prontuarios (em construção)
- Registrar Atendimentos (em construção)
- Estoque (Medicamentos e Vacinas)

### Perfil: Gerente
- Acesso total a todas as funcionalidades
- Gerenciar veterinarios
- Gerenciar usuarios
- Estoque completo
- Relatórios (em construção)

## Dados Iniciais

O sistema já vem com dados hardcoded para teste:

**usuarios:**
- Maria Silva (Recepcionista)
- Dr. João Santos (veterinario)
- Ana Costa (Gerente)

**Dados Cadastrados:**
- 2 Tutores
- 3 Pacientes
- 2 veterinarios

## Tecnologias

- C# Console Application
- Arquitetura baseada em CRUD
- Interface com molduras ASCII
