import { Input } from '@nextui-org/input';
import { IconEye, IconEyeOff } from '@tabler/icons-react';
import { useState } from 'react';

export default function PasswordInput ({ ...rest }) {
  const [isVisible, setIsVisible] = useState(false);

  const toggleVisibility = () => setIsVisible(!isVisible);

  return (
    <Input endContent={
      <button aria-label="toggle password visibility" className="focus:outline-none" type="button"
              onClick={toggleVisibility}>
        {isVisible
          ? (
            <IconEyeOff className="text-2xl text-default-400 pointer-events-none" />
            )
          : (
            <IconEye className="text-2xl text-default-400 pointer-events-none" />
            )}
      </button>
    } type={isVisible ? 'text' : 'password'} {...rest} />
  );
}
