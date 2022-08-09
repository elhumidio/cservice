
Read more at: [One Click Deployment YTT Documentation](https://eureka.stepstone.com/display/RM/One+Click+Deployment+YTT+Documentation)

## What is YTT? How do I use it?

With our new approach, we're giving the developer freedom to manipulate their CI/CD pipelines in Bamboo. YTT is a language that allows us to override variables and tasks within the Bamboo YAML configuration. As we mentioned above, we'll provide you with a `bamboo.yml` file in your application.

As you can see, there's a lot of code here, but don't fear. I will breakdown the code by section, which I have separated with `#! -------------------------------------`

- **Section 1**
	- These lines are to pull in sections that we have defined in our common templates.
	- As you can see in the flow diagram, we are merging files. So this configuration is telling our Bamboo plugin to merge `bamboo.yml` with the defined sections.
- **Section 2**
	- We will predefine the necessary variables for you. This includes your `project_key` and `service_name` which you may recognise from Bamboo.
	- We also have some functions defined names `*_overrides()`. These will allow you to override steps in Bamboo. One for CI and one for CD
- **Section 3**
	- We're just telling our plugin how to display our sections in the YAML output.
	- Notice we're also passing in the `*_overrides()` functions into our common sections.

**Section 2** is the only section you will need to pay any attention to.

By running this `bamboo.yml` file through our plugin, it will produce an output like the following..

```yaml
version: 2
plan:
  project-key: ZP
  key: SAVEDJOBSAPIZERO
  name: saved-jobs-api-zero
repositories:
- saved-jobs-api-zero:
    type: bitbucket-server
    server: Stash
    project: ZP
    slug: saved-jobs-api-zero
    clone-url: ssh://git@stash.stepstone.com:7999/zp/saved-jobs-api-zero.git
    public-key: ...
    private-key: ...
    branch: master
    command-timeout-minutes: "180"
    use-shallow-clones: "false"
    cache-on-agents: "false"
    submodules: "false"
    ssh-key-applies-to-submodules: "false"
    verbose-logs: "false"
    fetch-all: "false"
    lfs: "false"
...
```

This is the `bamboo.yaml` file that will be sent to Bamboo using the Linked Repository. As you can see, our variables have made it to the final output.

One of the benefits of using YTT is that we can follow the exact specifications of [Bamboo's YAML documentation](https://docs.atlassian.com/bamboo-specs-docs/6.3.0/specs-yaml.html)

## I want to override some tasks, where can I find this output document?

In Bamboo, we are able to export our CI/CD pipelines as YAML. There are a few steps to get this output.

1. Navigate to your project in Bamboo (e.g https://bamboo.stepstone.com/browse/ZP-SAVEDJOBSAPIZERO)
2. In the top-right, click Actions -> View plan configuration (e.g https://bamboo.stepstone.com/chain/admin/config/defaultStages.action?buildKey=ZP-SAVEDJOBSAPIZERO)
3. Then again, click Actions -> View plan as YAML (e.g https://bamboo.stepstone.com/exportSpecs/planYaml.action?buildKey=ZP-SAVEDJOBSAPIZERO)

Once you have this YAML document, you can use it as a reference for your YTT overrides. 

## Common YTT Snippets

### Adding additional variables

So, let's say you want to add a new variable to your project.

You can do this by defining an override. Create a new YTT definition to contain your overrides.

```yaml
#@ def build_plan_overrides():
variables: 
  #! Add an additional variable
  #@overlay/match missing_ok=True
  dummy: 'added'
#@ end
```

In the above example, we are matching the shape of the common YAML file. `variables` is a root element in the common YAML file, so in our definition, we must match.

In the example we are adding a brand new variable named `dummy`. This variable will be appended to the existing list of variables in our common YAML template.

You'll notice that we are using a YTT decorator, `#@overlay/match missing_ok=True`. This tells YTT that we want to append this variable even though it doesn't already exist. As the YTT default state is to override a variable, which we will cover next.

### Overriding a variable

Now we can override a variable. To do this, we can use the same override definition.

```yaml
#@ def build_plan_overrides():
variables:
  #! Add an additional variable
  #@overlay/match missing_ok=True
  dummy: 'added'

  #! Override a variable
  managed_by: 'ZP'
#@ end
```

`managed_by` is an existing variable in our common YAML script. By using the above configuration, we can change the value of the existing variable to `ZP`.

### Adding new tasks to jobs

Now that the basics are out of the way, here's something slightly more complicated.

As described above, our overrides need to match the shape of the common YAML template. When we say shape, we mean indentation.

Here's how we add a new task to a Job.

```yaml
#@ def build_plan_overrides():
Test and artifacts:
  tasks:

  #! Adding a new task
  #@overlay/match by=overlay.index(0)
  #@overlay/insert before=True
  - script:
      interpreter: SHELL
      scripts:
      - |
        #!/bin/bash

        echo "hello world"
      working-dir: service
      description: Hello World override
#@ end
```

The example above we are targeting the `Test and artifacts` block. We're then telling YTT to select the first item listed under `tasks` and prepend a `script`. The `script` is defined below the `#@overlay...` lines.

The code above will produce an output as follows:

```yaml
...
Test and artifacts:
  key: TESTS
  other:
    clean-working-dir: true
  docker:
    image: ${bamboo.build.dockerCiImage}
    volumes:
      ${bamboo.working.directory}: ${bamboo.working.directory}
      ${bamboo.tmp.directory}: ${bamboo.tmp.directory}
      /var/run/docker.sock: /var/run/docker.sock
    docker-run-arguments: []
  tasks:
  - script:
      interpreter: SHELL
      scripts:
      - |
        #!/bin/bash
  
        echo "hello world"
      working-dir: service
      description: Hello World override
  - checkout:
      repository: application_repository
      path: service
      force-clean-build: "true"
      description: Source Code Checkout
...
```

You can see that we have injected our own task above the `checkout` task.

### Overriding existing tasks on jobs

We can also override existing tasks within jobs. This is useful if you want to edit how a script works.

Take the following example:

```yaml
#@ def build_plan_overrides():
#! Replace a task in a job
Shared Veracode Scan:
  key: VERA
  other:
    clean-working-dir: true
  tasks:
  #@overlay/match by=overlay.index(0)
  #@overlay/replace
  - script:
      scripts:
      - "#!/bin/bash\n\n echo 'Overriden'"
      description: Veracode Create Application Override
#@ end
```

The above example is targeting the first `script` in the list of `tasks` and using `#@overlay/replace` to replace the content of the task with the `script` specified.

This will produce the following output:

```yaml
...
Shared Veracode Scan:
  key: VERA
  other:
    clean-working-dir: true
  tasks:
  - script:
      scripts:
      - |-
        #!/bin/bash
  
         echo 'Overriden'
      description: Veracode Create Application Override
  - script:
      interpreter: SHELL
      scripts:
      - "#!/bin/bash\n\nif [ \"$(ps -p \"$$\" -o comm=)\" != ..."
      argument: --ignore_errors=...
      working-dir: service
      description: Veracode Pipeline Scan
  requirements:
  - aws_group: default
  artifact-subscriptions:
  - artifact: veracode
    destination: service
...
```

### Adding a new job

With YTT, we can also add a brand new job to the Bamboo pipeline. This is useful if you want to add in a feature that doesn't fit the role of an existing job.

To do this, we must first inject a new job definition into a Bamboo stage.

```yaml
#@ def build_plan_overrides():
#! Add a brand new Job
stages:
#@overlay/match by=overlay.index(0)
- Build variables:
    jobs:
    #@overlay/match by=overlay.index(0)
    #@overlay/insert after=True
    - Inject build variables 2
#@ end
```

What we are doing in the above example is selecting the `stages` root element and then filtering down to the `Build variables` stage. We are then appending a new job to the list of jobs under the given stage.

The above YTT configuration will produce the following output when combined with our common template.

```yaml
version: 2
plan:
  project-key: ZP
  key: YTTPOCWEBAPI
  name: ytt-poc-webapi
  description: Inherited from yaml common
stages:
- Build variables:
    manual: false
    final: false
    jobs:
    - Inject build variables
    - Inject build variables 2
...
```

Unfortunately, we need to keep the original job as if deleted, will produce an invalid YAML file in the eyes of Bamboo. We can, however, 'disable' a job, which you will see in the next example.

To then add our new `Inject build variables 2` job, we can add the following to our overrides.

```yaml
#@ def build_plan_overrides():
#@overlay/match missing_ok=True
Inject build variables 2:
  tasks:
  - script:
      interpreter: SHELL
      scripts:
      - |
        #!/bin/bash
        echo "New script!!"
      working-dir: service
      description: New script
#@ end
```

This will add a brand new job to the common YAML template. Notice we have `missing_ok=True` to indicate that we are happy this job does not already exist.

Combined, your overrides should look like the following:

```yaml
#@ def build_plan_overrides():
#! Add a brand new Job
stages:
#@overlay/match by=overlay.index(0)
- Build variables:
    jobs:
    #@overlay/match by=overlay.index(0)
    #@overlay/insert after=True
    - Inject build variables 2

#@overlay/match missing_ok=True
Inject build variables 2:
  tasks:
  - script:
      interpreter: SHELL
      scripts:
      - |
        #!/bin/bash
        echo "New script!!"
      working-dir: service
      description: New script
#@ end
```

### Disabling a job

As mentioned previously, we can't really delete a job easily.

To disable a job, we're using the below recommendation:

```yaml
#@ def build_plan_overrides():
#! Disable a job
Inject build variables:
  #@overlay/replace
  tasks:
  - script:
      interpreter: SHELL
      scripts:
      - |
        #!/bin/bash
        echo "*** JOB DISABLED ***"
      working-dir: service
      description: DISABLED
#@ end
```

In the above example, we are using `#@overlay/replace` which is telling YTT to replace **everything** in the `tasks` list with the `script` we are defining.

This will produce an output as follows:

```yaml
#@ def build_plan_overrides():
Inject build variables:
  key: IBV
  other:
    clean-working-dir: true
  tasks:
  - script:
      interpreter: SHELL
      scripts:
      - |
        #!/bin/bash
        echo "*** JOB DISABLED ***"
      working-dir: service
      description: DISABLED
  requirements:
  - aws_group: default
#@ end
```

### Where can I see these examples in action?

Here's a config file we have used for building this POC:

https://stash.stepstone.com/projects/ZP/repos/ytt-poc-webapi/browse/bamboo-config/webapi-overrides.yaml

# Most Common use cases

### Enabling Release Tests for your plan
Ok, but what are release tests? The answer for this and much more you will find here: https://eureka.stepstone.com/display/RM/Release+Tests  

To enable Release Tests for you application, the thing you need to do is adding the following lines to the bamboo.yml file.  
In loads section:
```yaml
#@ load("/plan/_templates/release-tests/template.lib.yaml", "build_plan_release_tests")
```
And in template section:
```yaml
--- #@ template.replace([overlay.apply(build_plan_release_tests(config), release_tests_overrides())])
--- #@ plan_permissions(config, "-release-tests")
```
There is also a possibility to change docker image for running release tests by overwriting variable *dockerRtImage*, as it is done in the provided example.  


Example change: https://stash.stepstone.com/projects/ZP/repos/ytt-poc-webapi/commits/07c8b3b0d359445803c8e9aea67314e28604d78d

