import * as React from 'react';

export interface Props {
  name: string;
  onIncrement?: () => void;
  onDecrement?: () => void;
}

function Counter({ name, onIncrement, onDecrement }: Props) {
  return (
    <div>
      <div>
        {name}
      </div>
      <div>
        <button onClick={onDecrement}>-</button>
        <button onClick={onIncrement}>+</button>
      </div>
    </div>
  );
}

export default Counter;