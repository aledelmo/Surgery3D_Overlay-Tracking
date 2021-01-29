import yaml
try:
    from yaml import CLoader as Loader, CDumper as Dumper
except ImportError:
    from yaml import Loader, Dumper


def get_config_from_file(config: str) -> dict:
    """
        Server-side configuration
        :param config: config YAML filepath
        :return: configuration dictionary
        """
    with open(config) as f:
        return yaml.load(f, Loader=Loader)
